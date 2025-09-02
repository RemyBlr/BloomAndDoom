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
    private bool settingsSceneLoaded = false;

    public bool IsPaused => isPaused;
    public bool IsConfirmingQuit => isConfirmingQuit;
    public bool AreSettingsOpen => settingsSceneLoaded;

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
            if (settingsSceneLoaded)
                CloseSettings();
            else if (isConfirmingQuit)
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

    public void ConfirmQuit() {        
        // Set time to normal speed
        Time.timeScale = 1f;

        if (GameStats.Instance != null)
            GameStats.Instance.EndSession();
        
        DisableGameUI();
        SceneManager.LoadScene(mainMenuSceneName);
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
            MonoBehaviour[] components = GameManager.Instance.playerInstance.GetComponents<MonoBehaviour>();
            
            foreach (MonoBehaviour component in components) {
                var controlsField = component.GetType().GetField("controls", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (controlsField != null && controlsField.FieldType == typeof(PlayerControls)) {
                    PlayerControls controls = (PlayerControls)controlsField.GetValue(component);
                    if (controls != null)
                        controls.Disable();
                }
            }
        }
    }

    private void EnablePlayerControls() {
        if (GameManager.Instance?.playerInstance != null) {
            MonoBehaviour[] components = GameManager.Instance.playerInstance.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components) {
                var controlsField = component.GetType().GetField("controls", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (controlsField != null && controlsField.FieldType == typeof(PlayerControls)) {
                    PlayerControls controls = (PlayerControls)controlsField.GetValue(component);
                    if (controls != null)
                        controls.Enable();

                }
            }
        }
    }

    public void ShowQuitConfirmation() {
        isConfirmingQuit = true;
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        if (confirmQuitUI != null)
            confirmQuitUI.SetActive(true);
    }

    public void OpenSettings() {       

        if (settingsSceneLoaded) return;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        settingsSceneLoaded = true;
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);

        SceneManager.sceneLoaded += OnSettingsSceneLoaded;
    }

    private void OnSettingsSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == settingsSceneName) {
            Debug.Log("Scène des paramètres chargée");
            
            SceneManager.sceneLoaded -= OnSettingsSceneLoaded;
            
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects) {
                SettingsManager settingsManager = obj.GetComponentInChildren<SettingsManager>();
                if (settingsManager != null) {
                    settingsManager.SetCameFromGame(true);
                    Debug.Log("SettingsManager configuré pour retour au jeu");
                    break;
                }
            }
        }
    }

    public void CloseSettings() {
        if (!settingsSceneLoaded) return;
        
        Debug.Log("Fermeture des paramètres");
        
        settingsSceneLoaded = false;
        
        SceneManager.UnloadSceneAsync(settingsSceneName);
        
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    void OnDestroy() { Time.timeScale = 1f; }

    private void DisableGameUI() {
        Canvas[] allCanvas = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvas) {
            if (canvas.gameObject != pauseMenuUI?.transform.parent?.gameObject && canvas.gameObject != confirmQuitUI?.transform.parent?.gameObject)
                canvas.gameObject.SetActive(false);
        }
    }
}