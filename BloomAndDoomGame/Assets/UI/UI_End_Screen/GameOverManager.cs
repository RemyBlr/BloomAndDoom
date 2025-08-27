using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Texts")]
    public TextMeshProUGUI duration;
    public TextMeshProUGUI damageDealt;
    public TextMeshProUGUI damageTaken;
    public TextMeshProUGUI goldEarned;
    public TextMeshProUGUI distanceTraveled;

    [Header("Items")]
    public Transform itemsParent;
    public GameObject itemUIPrefab;

    [Header("Navigation")]
    public string mainMenu = "MainMenu";
    public string playAgain = "CharacterSelectionScene";
    /*
    void Start()
    {
        duration.text = $"Durée : {GameStats.Instance.duration:F1} sec";
        damageDealt.text = $"Dégâts infligés : {GameStats.Instance.damageDealt}";
        damageTaken.text = $"Dégâts reçus : {GameStats.Instance.damageTaken}";
        goldEarned.text = $"Argent gagné : {GameStats.Instance.goldEarned}";
        distanceTraveled.text = $"Distance parcourue : {GameStats.Instance.distanceTraveled}";

        foreach (var item in GameStats.Instance.collectedItems)
        {
            GameObject go = Instantiate(itemUIPrefab, itemsParent);
            go.GetComponentInChildren<TextMeshProUGUI>().text = item;
        }
    }

    public void PlayAgain() {
        SceneManager.LoadScene(playAgain);
    }

    public void GoMainMenu() {
        SceneManager.LoadScene(mainMenu);
    }
    */
}
