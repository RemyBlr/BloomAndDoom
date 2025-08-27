using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;
    private GameObject settingsPanel;

    [Header("Navigation")]
    public string charaSelec = "CharacterSelectionScene";
    public string settingsMenuScene = "SettingsMenuScene";

    public void newGame() {
        Debug.Log("New game");

        // FOR TESTING PRURPOSE, DELETES ALL PLAYERPREFS
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

        SceneManager.LoadScene(charaSelec);
    }

    public void openSettings() {
        Debug.Log("Open settings");
        SceneManager.LoadScene(settingsMenuScene);
    }

    public void quitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
