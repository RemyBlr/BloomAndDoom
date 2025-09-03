using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUIController : MonoBehaviour
{
    [Header("Boutons du Menu Principal")]
    public Button resumeButton;
    public Button settingsButton;
    public Button quitButton;
    
    [Header("Boutons de Confirmation")]
    public Button confirmQuitButton;
    public Button cancelQuitButton;
    
    private GamePauseManager pauseManager;

    void Start() {
        pauseManager = GamePauseManager.Instance;
        
        if (pauseManager == null) return;        
        SetupButtons();
    }

    void SetupButtons() {
        // Main menu
        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => pauseManager.ResumeGame());
            
        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => pauseManager.OpenSettings());
            
        if (quitButton != null)
            quitButton.onClick.AddListener(() => pauseManager.ShowQuitConfirmation());
            
        // Confirm quit
        if (confirmQuitButton != null)
            confirmQuitButton.onClick.AddListener(() => pauseManager.ConfirmQuit());
            
        if (cancelQuitButton != null)
            cancelQuitButton.onClick.AddListener(() => pauseManager.CancelQuit());
    }

    void OnDestroy() {
        if (resumeButton != null)
            resumeButton.onClick.RemoveAllListeners();
            
        if (settingsButton != null)
            settingsButton.onClick.RemoveAllListeners();
            
        if (quitButton != null)
            quitButton.onClick.RemoveAllListeners();
            
        if (confirmQuitButton != null)
            confirmQuitButton.onClick.RemoveAllListeners();
            
        if (cancelQuitButton != null)
            cancelQuitButton.onClick.RemoveAllListeners();
    }
}