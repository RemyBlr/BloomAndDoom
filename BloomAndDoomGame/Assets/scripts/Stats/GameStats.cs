using UnityEngine;
using System;

[System.Serializable]
public class GameSession
{
    public string characterClassName;

    // Timer
    public float startTime;
    public float endTime;
    public float GetSessionDuration() => endTime - startTime;
    
    // Atk stats
    public float totalDamageDealt;
    public float totalDamageTaken;
    public int enemiesKilled;
    
    // Progress
    public int finalLevel;
    public float distanceTraveled;
    
    // Curreny
    public int currencyGained;
}

//-------------------------------------------------------------------------------------
// This class saves and calculates actions done by the player
// Gives functions that return end game stats for the EndGame panel to show
//-------------------------------------------------------------------------------------
public class GameStats : MonoBehaviour
{
    [Header("Current Session")]
    [SerializeField]
    private GameSession currentSession;
    private Vector3 lastPosition;
    private bool sessionActive = false;
    
    // Notifiers
    public static event Action<GameSession> OnSessionStart;
    public static event Action<GameSession> OnSessionEnd;
    public static event Action<GameSession> OnStatUpdated;

    public static GameStats Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (GameManager.Instance?.playerInstance != null)
            StartSession();
    }

    void Update()
    {
        if (sessionActive && GameManager.Instance?.playerInstance != null)
            CalcDistance();
    }

    //---------------- Session ----------------
    public void StartSession()
    {
        if (sessionActive) return;
        
        currentSession = new GameSession();
        currentSession.sessionStartTime = Time.time;
        
        if (GameManager.Instance?.playerInstance != null)
        {
            CharacterStats playerStats = GameManager.Instance.playerInstance.GetComponent<CharacterStats>();
            if (playerStats != null && playerStats.GetCharacterClass() != null)
            {
                currentSession.characterClassName = playerStats.GetCharacterClass().className;
                currentSession.finalLevel = playerStats.GetLevel();
            }
            
            lastPosition = GameManager.Instance.playerInstance.transform.position;
        }
        
        sessionActive = true;
        OnSessionStart?.Invoke(currentSession);
    }

    public void EndSession()
    {
        if (!sessionActive) return;
        
        currentSession.sessionEndTime = Time.time;
        
        if (GameManager.Instance?.playerInstance != null)
        {
            CharacterStats playerStats = GameManager.Instance.playerInstance.GetComponent<CharacterStats>();
            if (playerStats != null)
                currentSession.finalLevel = playerStats.GetLevel();
        }
        
        sessionActive = false;
        OnSessionEnd?.Invoke(currentSession);
    }

    //---------------- Distance ----------------
    private void CalcDistance()
    {
        if (GameManager.Instance?.playerInstance == null) return;
        
        Vector3 currentPosition = GameManager.Instance.playerInstance.transform.position;
        float distance = Vector3.Distance(lastPosition, currentPosition);
        currentSession.distanceTraveled += distance;
        lastPosition = currentPosition;
    }

    //---------------- Update stats ----------------
    public void AddDamageDealt(float damage)
    {
        if (!sessionActive) return;
        currentSession.totalDamageDealt += damage;
        OnStatsUpdated?.Invoke(currentSession);
    }

    public void AddDamageTaken(float damage)
    {
        if (!sessionActive) return;
        currentSession.totalDamageTaken += damage;
        OnStatsUpdated?.Invoke(currentSession);
    }

    public void AddEnemyKilled()
    {
        if (!sessionActive) return;
        currentSession.enemiesKilled++;
        OnStatsUpdated?.Invoke(currentSession);
    }

    public void AddCurrencyGained(int currency)
    {
        if (!sessionActive) return;
        currentSession.currencyGained += currency;
        OnStatsUpdated?.Invoke(currentSession);
    }

    //---------------- End Game panel ----------------
    public GameSession GetCurrentSession() => currentSession;
    public bool IsSessionActive() => sessionActive;

    public string GetFormattedSummary()
    {
        // TODO complete when merged with end game panel
        return "";
    }
}
