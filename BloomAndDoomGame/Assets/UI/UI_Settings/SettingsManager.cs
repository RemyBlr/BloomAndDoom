using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider slider;

    [Header("Graphics")]
    public TMP_Dropdown drowpdown;

    [Header("Navigation")]
    public string mainMenuSceneName = "MainMenu";
    
    [Header("Navigation Buttons")]
    public Button backToMainMenuButton;
    public Button backToGameButton;
    public GameObject backToMainMenuObject;
    public GameObject backToGameObject;

    private Resolution[] resolutions;
    private bool cameFromGame = false;

    void Start() {
        // Check if we come from game
        SetCameFromGame(false);

        // Resolutions
        resolutions = Screen.resolutions;
        drowpdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        drowpdown.AddOptions(options);

        // PlayerPrefs dropdown
        int savedResIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        drowpdown.value = savedResIndex;
        drowpdown.RefreshShownValue();
        drowpdown.onValueChanged.AddListener(SetResolution);

        // init volume
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        slider.value = savedVolume;
        SetVolume(savedVolume);
        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetCameFromGame(bool isFromGame) {
        cameFromGame = isFromGame;
        SetupBackButton();
    }
    
    void SetupBackButton() {
        if (cameFromGame) {
            // We come from game, show "Back to game"
            if (backToGameObject != null)
                backToGameObject.SetActive(true);

            if (backToMainMenuObject != null)
                backToMainMenuObject.SetActive(false);

            if (backToGameButton != null) {
                backToGameButton.onClick.RemoveAllListeners();
                backToGameButton.onClick.AddListener(ReturnToGame);
            }
        }
        else {
            // We come from menu, show "Back to menu
            if (backToGameObject != null)
                backToGameObject.SetActive(false);

            if (backToMainMenuObject != null)
                backToMainMenuObject.SetActive(true);
                
            if (backToMainMenuButton != null) {
                backToMainMenuButton.onClick.RemoveAllListeners();
                backToMainMenuButton.onClick.AddListener(BackToMainMenu);
            }
        }
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetResolution(int resIndex) {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resIndex);
    }

    public void BackToMainMenu() {
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void ReturnToGame() {
        string returnScene = PlayerPrefs.GetString("ReturnToScene", "");
                
        GamePauseManager pauseManager = FindObjectOfType<GamePauseManager>();
        if (pauseManager != null)
            pauseManager.CloseSettings();
        else{
            Debug.LogWarning("Why do i come here ??!!?!?!?");
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }

    void OnDestroy() {
        if (drowpdown != null)
            drowpdown.onValueChanged.RemoveAllListeners();
        if (slider != null)
            slider.onValueChanged.RemoveAllListeners();
    }
}