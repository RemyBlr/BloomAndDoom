using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public Sprite icon;
    public bool unlockedByDefault;
    public int price;
}


[System.Serializable]
public class Spell {
    public Sprite icon;
    public string description;
}

[CreateAssetMenu(fileName="NewCharacterClass", menuName="Character/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    [Header("Info")]
    public string className;
    
    [Header("Stats")]
    public int hp;
    public int atk;
    public int spd;

    [Header("Preview")]
    public Sprite portrait;
    public GameObject prefabPreview;
    public GameObject prefab;

    [Header("Spells")]
    public Spell[] spells = new Spell[3];

    [Header("Weapons")]
    public Weapon[] weapons;
}
