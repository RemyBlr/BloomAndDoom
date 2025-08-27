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

    //
    public int currencySpent;
    public int experienceGained;
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

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        if (GameManager.Instance?.character != null)
            StartSession();
    }

    void Update() {
        if (sessionActive && GameManager.Instance?.character != null)
            CalcDistance();
    }

    //---------------- Session ----------------
    public void StartSession()
    {
        if (sessionActive) return;
        
        currentSession = new GameSession();
        currentSession.startTime = Time.time;
        
        if (GameManager.Instance?.character != null) {
            CharacterStats playerStats = GameManager.Instance.character.GetComponent<CharacterStats>();
            if (playerStats != null && playerStats.GetCharacterClass() != null) {
                currentSession.characterClassName = playerStats.GetCharacterClass().className;
                currentSession.finalLevel = playerStats.GetLevel();
            }
            
            lastPosition = GameManager.Instance.character.transform.position;
        }
        
        sessionActive = true;
        OnSessionStart?.Invoke(currentSession);
    }

    public void EndSession()
    {
        if (!sessionActive) return;
        
        currentSession.endTime = Time.time;
        
        if (GameManager.Instance?.character != null) {
            CharacterStats playerStats = GameManager.Instance.character.GetComponent<CharacterStats>();
            if (playerStats != null)
                currentSession.finalLevel = playerStats.GetLevel();
        }
        
        sessionActive = false;
        OnSessionEnd?.Invoke(currentSession);
    }

    //---------------- Distance ----------------
    private void CalcDistance() {
        if (GameManager.Instance?.character == null) return;
        
        Vector3 currentPosition = GameManager.Instance.character.transform.position;
        float distance = Vector3.Distance(lastPosition, currentPosition);
        currentSession.distanceTraveled += distance;
        lastPosition = currentPosition;
    }

    //---------------- Update stats ----------------
    public void AddDamageDealt(float damage) {
        if (!sessionActive) return;
        currentSession.totalDamageDealt += damage;
        OnStatUpdated?.Invoke(currentSession);
    }

    public void AddDamageTaken(float damage) {
        if (!sessionActive) return;
        currentSession.totalDamageTaken += damage;
        OnStatUpdated?.Invoke(currentSession);
    }

    public void AddEnemyKilled() {
        if (!sessionActive) return;
        currentSession.enemiesKilled++;
        OnStatUpdated?.Invoke(currentSession);
    }

    public void AddCurrencyGained(int currency) {
        if (!sessionActive) return;
        currentSession.currencyGained += currency;
        OnStatUpdated?.Invoke(currentSession);
    }

    //---------------- Used in CharacterStats ----------------
    public void AddExperienceGained(int xp) {
        if (!sessionActive) return;
        currentSession.experienceGained += xp;
        OnStatUpdated?.Invoke(currentSession);
    }

    public void AddCurrencySpent(int currency) {
        if (!sessionActive) return;
        currentSession.currencySpent += currency;
        OnStatUpdated?.Invoke(currentSession);
    }

    //---------------- End Game panel ----------------
    public GameSession GetCurrentSession() => currentSession;
    public bool IsSessionActive() => sessionActive;

    public string GetFormattedSummary() {
        // TODO complete when merged with end game panel
        // can get info with currentSession.finalLevel
        return "";
    }
}
