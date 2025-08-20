using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI name;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI spd;
    public Transform buttonParent;
    public GameObject buttonPrefab;

    [Header("Spells")]
    public Image[] spellIcons;
    public TextMeshProUGUI[] spellDescription;

    [Header("Preview")]
    public Transform previewStartPoint;
    private GameObject currentPreview;

    private CharacterClass[] classes;
    private string mainMenuSceneName = "MainMenu";
    private string mapSceneName = "ProceduralGeneration";

    // claled when script is laoded
    void Awake() {
        // loads all classes
        classes = Resources.LoadAll<CharacterClass>("Classes");

        // create button for each class
        for(int i = 0; i < classes.Length; ++i) {
            int index = i;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            // prefab script
            var btnUI = newButton.GetComponent<ClassButtonUI>();
            if(btnUI != null && classes[i].portrait != null)
                btnUI.portrait.sprite = classes[i].portrait;

            newButton.GetComponent<Button>().onClick.AddListener(() => SelectClass(index));
        }

        // First class selected by default
        if(classes.Length > 0) SelectClass(0);
    }

    private int currentIndex = 0;

    public void SelectClass(int index) {
        currentIndex = index;

        // Update UI
        CharacterClass picked = classes[index];
        name.text = picked.className;
        hp.text = $"Hp: {picked.hp}";
        atk.text = $"Atk: {picked.atk}";
        spd.text = $"Spd: {picked.spd}";

        // TODO : find better way to get the number of spells
        int numberOfSpells = 3;

        for(int i = 0; i < numberOfSpells; ++i) {
            if(i < picked.spells.Length) {
                spellIcons[i].sprite = picked.spells[i].icon;
                spellDescription[i].text = picked.spells[i].description;
            }
        }

        // Preview
        if(currentPreview != null) Destroy(currentPreview);
        currentPreview = Instantiate(picked.prefab, previewStartPoint.position, Quaternion.identity);

        // set picked class
        SelectedCharacter.pickedClass = picked;
    }

    // Update is called once per frame
    // In this case, turns the prefab
    void Update() {
        // mouse 0 is left click
        if(currentPreview != null) {
            // mouse x is horizontal mouvement
            float mouseX = Mouse.current.delta.ReadValue().x;
            if (mouseX != 0)
                currentPreview.transform.Rotate(Vector3.up, mouseX * 0.1f);
        }
    }

    public void BackToMenu() {
        Debug.Log("Back to menu from class selection");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void PlayGame() {
        Debug.Log("Start game");
        if(SelectedCharacter.pickedClass == null) {
            Debug.LogWarning("Aucune classe sélectionée");
            return;
        }
        SceneManager.LoadScene(mapSceneName);

    }
}
