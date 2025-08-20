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

    private Resolution[] resolutions;

    void Start() {
        // init resolutions
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

    public void SetVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        // save to PlayerPrefs
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetResolution(int resIndex) {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        // save to PlayerPrefs
        PlayerPrefs.SetInt("resolutionIndex", resIndex);
    }

    public void BackToMainMenu() {
        // save to disk
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
