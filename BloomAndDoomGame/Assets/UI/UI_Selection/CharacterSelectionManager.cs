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
    public Transform buttonParent;
    public GameObject buttonPrefab;

    [Header("Spells")]
    public Image[] spellIcons;
    public TextMeshProUGUI[] spellDescription;

    [Header("Weapons")]
    public Transform weaponsParent;
    public GameObject weaponUIPrefab;

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

            newButton.GetComponent<Button>().onClick.AddListener(() => SelectClass(index));
        }

        // First class selected by default
        if (classes.Length > 0)
        {
            SelectClass(0);
            GameManager.Instance.character = classes[0].prefab;
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
        name.text = picked.className;
        hp.text = $"Hp: {picked.hp}";
        atk.text = $"Atk: {picked.atk}";
        spd.text = $"Spd: {picked.spd}";

        GameManager.Instance.character = classes[index].prefab;
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

        currentPreview = Instantiate(picked.prefabPreview, previewStartPoint);

        // offset at start
        currentPreview.transform.localPosition = new Vector3(0f, -50f, -50f);

        // prefab faces us at start
        currentPreview.transform.localRotation = Quaternion.Euler(0, 180f, 0);

        //currentPreview.transform.localPosition = Vector3.zero;
        //currentPreview.transform.localRotation = Quaternion.identity;

        float scaleFactor = 100f;
        currentPreview.transform.localScale = Vector3.one * scaleFactor;

        // set picked class
        SelectedCharacter.pickedClass = picked;

        // print weapons
        foreach (Transform child in weaponsParent) Destroy(child.gameObject);

        Weapon defaultWeapon = null;

        foreach (var weapon in picked.weapons)
        {
            bool alreadyUnlocked = weapon.unlockedByDefault || SaveSystem.IsWeaponUnlocked(picked.className, weapon.weaponName);

            GameObject ui = Instantiate(weaponUIPrefab, weaponsParent);
            ui.GetComponent<WeaponUI>().Setup(weapon, picked, alreadyUnlocked);

            // Default weapon selected
            if (alreadyUnlocked && defaultWeapon == null)
                defaultWeapon = weapon;
        }

        // Select default weapon
        if (SelectedCharacter.selectedWeapon == null && defaultWeapon != null)
            SelectedCharacter.selectedWeapon = defaultWeapon;
        
        // Update UI
        foreach (Transform child in weaponsParent)
            child.GetComponent<WeaponUI>().UpdateUI();

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
