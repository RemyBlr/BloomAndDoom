using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI label;
    public Button button;

    private Weapon weapon;
    private CharacterClass character;
    private bool unlocked;

    public void Setup(Weapon weaponData, CharacterClass owner, bool isUnlocked)
    {
        weapon = weaponData;
        character = owner;
        unlocked = isUnlocked;

        icon.sprite = weapon.icon;
        UpdateUI();

        button.onClick.RemoveAllListeners();
        if (unlocked)
            button.onClick.AddListener(SelectWeapon);
        else
            button.onClick.AddListener(BuyWeapon);
    }

    void UpdateUI()
    {
        if (unlocked)
            label.text = "Debloque";
        else
        {
            label.text = $"Prix: {weapon.price}";
        }
    }

    void BuyWeapon()
    {
        // TODO
    }

    void SelectWeapon()
    {
        SelectedCharacter.selectedWeapon = weapon;
        Debug.Log($"Arme sélectionnée : {weapon.weaponName}");
    }
}
