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
        // hide main menu
        mainMenuPanel.SetActive(false);
        // show settings
        settingsPanel.SetActive(true);
    }

    public void closeSettings() {
        Debug.Log("Close settings");
        // hide settings
        settingsPanel.SetActive(false);
        // show main menu
        mainMenuPanel.SetActive(true);
    }

    public void quitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
