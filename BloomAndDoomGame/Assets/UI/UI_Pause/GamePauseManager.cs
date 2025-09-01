using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject confirmQuitUI;
    
    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string settingsSceneName = "SettingsMenuScene";
    
    private bool isPaused = false;
    private bool isConfirmingQuit = false;

    public bool IsPaused => isPaused;
    public bool IsConfirmingQuit => isConfirmingQuit;

    public static GamePauseManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Both menus are closed on start
    void Start() {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        if (confirmQuitUI != null)
            confirmQuitUI.SetActive(false);
    }

    void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            if (isConfirmingQuit)
                CancelQuit();
            else if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void CancelQuit() {
        isConfirmingQuit = false;
        
        if (confirmQuitUI != null)
            confirmQuitUI.SetActive(false);
            
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    public void ResumeGame() {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        if (confirmQuitUI != null)
            confirmQuitUI.SetActive(false);
            
        isConfirmingQuit = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        EnablePlayerControls();
    }

    public void PauseGame() {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
            
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        DisablePlayerControls();
    }

    private void DisablePlayerControls() {
        if (GameManager.Instance?.playerInstance != null) {
            // TODO adapt for common actions
            ArcherActions archerActions = GameManager.Instance.playerInstance.GetComponent<ArcherActions>();
            if (archerActions != null)
                // Comes from ArcherActions
                archerActions.SetControlsEnabled(false);
        }
    }

    private void EnablePlayerControls() {
        if (GameManager.Instance?.playerInstance != null) {
            // TODO adapt for common actions
            ArcherActions archerActions = GameManager.Instance.playerInstance.GetComponent<ArcherActions>();
            if (archerActions != null)
                // Comes from ArcherActions
                archerActions.SetControlsEnabled(true);
        }
    }

    public void ShowQuitConfirmation() {
        isConfirmingQuit = true;
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        if (confirmQuitUI != null)
            confirmQuitUI.SetActive(true);
    }

    public void ConfirmQuit() {        
        // Set time to normal speed
        Time.timeScale = 1f;
        if (GameStats.Instance != null)
            GameStats.Instance.EndSession();
        
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OpenSettings() {        
        // Set time to normal speed
        Time.timeScale = 1f;
        
        // Save scene
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("ReturnToScene", currentScene);
        // Flag, we coming from game
        PlayerPrefs.SetInt("CameFromGame", 1); 
        PlayerPrefs.Save();
        
        SceneManager.LoadScene(settingsSceneName);
    }
    
    public void TogglePause() {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void OnDestroy() { Time.timeScale = 1f; }
}