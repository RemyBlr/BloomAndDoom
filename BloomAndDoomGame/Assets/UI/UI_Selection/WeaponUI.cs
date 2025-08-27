using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI price;
    public Button button;

    private Weapon weapon;
    private CharacterClass character;
    private bool unlocked;

    public void Setup(Weapon weaponData, CharacterClass owner, bool isUnlocked)
    {
        weapon = weaponData;
        character = owner;
        unlocked = isUnlocked;

        if(icon != null)
            icon.sprite = weapon.icon;

        UpdateUI();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    void OnClick() {
        if(!unlocked)
            BuyWeapon();
        else if(SelectedCharacter.selectedWeapon != weapon)
            SelectWeapon();
    }

    public void UpdateUI() {
        if (!unlocked) {
            if(price != null)
                price.text = $"Prix: {weapon.price}";

            // button enabled
            button.interactable = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Acheter";
        }
        else
        {
            if(price != null)
                price.text = "";
            
            if(SelectedCharacter.selectedWeapon == weapon) {
                // disable button
                button.interactable = false;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Selectionnee";
            }
            else
            {
                // enable button
                button.interactable = true;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Selectionner";
            }
        }
    }

    void BuyWeapon()
    {
        Debug.Log($"Achat arme : {weapon.weaponName} pour {weapon.price} $");

        // TODO check if enoguh money

        unlocked = true;
        SaveSystem.UnlockWeapon(character.className, weapon.weaponName);

        // select weapon after buying
        SelectWeapon();
    }

    void SelectWeapon()
    {
        SelectedCharacter.selectedWeapon = weapon;
        Debug.Log($"Arme sélectionnée : {weapon.weaponName}");

        foreach (Transform sibling in transform.parent) {
            var ui = sibling.GetComponent<WeaponUI>();
            if (ui != null) ui.UpdateUI();
        }
    }
}
