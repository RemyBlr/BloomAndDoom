using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    public void newGame() {
        Debug.Log("New game");
        SceneManager.LoadScene("SampleScene");
    }

    public void openSettings() {
        Debug.Log("Open settings");
        SceneManager.LoadScene("SettingsMenuScene");
    }

    public void quitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
