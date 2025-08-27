using System;
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
    public TextMeshProUGUI def;
    public TextMeshProUGUI critChance;
    public TextMeshProUGUI critDamage;
    public Transform buttonParent;
    public GameObject buttonPrefab;

    [Header("Spells")]
    public Image[] spellIcons;
    public TextMeshProUGUI[] spellDescription;

    [Header("Preview")]
    public Transform previewStartPoint;
    private GameObject currentPreview;

    private CharacterClass[] classes;
    private ClassButtonUI[] classButtons;
    private string mainMenuSceneName = "MainMenu";
    private string mapSceneName = "GameScene";

    // claled when script is laoded
    void Awake() {
        // loads all classes
        classes = Resources.LoadAll<CharacterClass>("Classes");

        // create button for each class
        classButtons = new ClassButtonUI[classes.Length];

        for(int i = 0; i < classes.Length; ++i) {
            int index = i;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            // prefab script
            var btnUI = newButton.GetComponent<ClassButtonUI>();
            if(btnUI != null && classes[i].portrait != null)
                btnUI.portrait.sprite = classes[i].portrait;

            newButton.GetComponent<Button>().onClick.AddListener(() => SelectClass(index));

            classButtons[i] = btnUI;

            //newButton.GetComponent<Button>().onClick.AddListener(() => SelectClass(index));
        }

        // First class selected by default
        if (classes.Length > 0) {
            SelectClass(0);
            //GameManager.Instance.character = classes[0].prefab;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (arg0, mode) =>
        {
            GameManager.Instance.InstantiatePlayer();
        };
    }

    public void SelectClass(int index) {

        // Add border to selected class
        for (int i = 0; i < classButtons.Length; i++) {
            if (classButtons[i] != null) {
                bool selected = (i == index);
                classButtons[i].SetSelected(selected);
                
                Debug.Log($"Button {i} selected = {selected}");
            }
        }

        // Update UI
        CharacterClass picked = classes[index];
        DisplayClassStats(picked);

<<<<<<< HEAD
        GameManager.Instance.character = picked.prefab;
        // TODO : find better way to get the number of spells
        int numberOfSpells = 3;

        for(int i = 0; i < numberOfSpells; ++i) {
            if(i < picked.spells.Length) {
                spellIcons[i].sprite = picked.spells[i].icon;
                spellDescription[i].text = picked.spells[i].description;
            }
        }
=======
        //GameManager.Instance.character = classes[index].prefab;
>>>>>>> main

        // Preview
        if(currentPreview != null) Destroy(currentPreview);
        CreatePreview(picked);
       
        // set picked class
        SelectedCharacter.pickedClass = picked;
    }

    private void DisplayClassStats(CharacterClass characterClass) {
        name.text = characterClass.className;
        
        // Get base stats at start of game
        CharacterBaseStats baseStats = characterClass.GetStatsAtLevel(1);
        
        // Display stats
        hp.text = $"HP: {baseStats.hp:F0}";
        atk.text = $"ATK: {baseStats.atk:F0}";
        spd.text = $"SPD: {baseStats.spd:F0}";
        def.text = $"DEF: {baseStats.def:F0}";
        critChance.text = $"CRIT: {baseStats.critChance:P0}";
        critDamage.text = $"CRIT DMG: +{baseStats.critDamage:P0}";
        
        // Display spells
        int numberOfSpells = 3;
        for(int i = 0; i < numberOfSpells; ++i) {
            if(i < characterClass.spells.Length) {
                spellIcons[i].sprite = characterClass.spells[i].icon;
                spellDescription[i].text = characterClass.spells[i].description;
            }
        }
    }

    private void CreatePreview(CharacterClass characterClass) {
        currentPreview = Instantiate(characterClass.prefabPreview, previewStartPoint);

        // offset at start
        currentPreview.transform.localPosition = new Vector3(0f, -50f, -50f);

        // prefab faces us at start
        currentPreview.transform.localRotation = Quaternion.Euler(0, 180f, 0);

        float scaleFactor = 100f;
        currentPreview.transform.localScale = Vector3.one * scaleFactor;

    }

    // Update is called once per frame
    // In this case, turns the prefab
    void Update() {
        // mouse 0 is left click
        if(currentPreview != null && Mouse.current.leftButton.isPressed) {
            // mouse x is horizontal mouvement
            float mouseX = Mouse.current.delta.ReadValue().x;
            if (mouseX != 0)
                currentPreview.transform.Rotate(Vector3.up, mouseX * -5f, Space.World);
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
