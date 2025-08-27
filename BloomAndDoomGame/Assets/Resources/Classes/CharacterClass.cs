using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CharacterBaseStats
{
    public float hp;
    public float atk;
    public float spd;
    public float def;
    public float critChance;
    public float critDamage;
}

[CreateAssetMenu(fileName="NewCharacterClass", menuName="Character/CharacterClass")]
public class CharacterClass : ScriptableObject
{
    [Header("Info")]
    public string className;
    
    [Header("Base stats")]
    public int hp;
    public int atk;
    public int spd;
    public int def;

    [Header("Advanced Stats")]
    [Range(0f, 1f)]
    public float critChance = 0.05f;
    public float critDamage = 0.5f;

    [Header("Resources")]
    public int startingLevel = 1;
    public int startingCurrency = 0;

    [Header("Per Level up bonus")]
    public float hpPerLevel = 10f;
    public float atkPerLevel = 2f;
    public float defPerLevel = 1f;
    public float spdPerLevel = 0.5f;

    [Header("Preview")]
    public Sprite portrait;
    public GameObject prefabPreview;
    public GameObject prefab;

    [System.Serializable]
    public class Spell {
        public Sprite icon;
        public string description;
    }

    [Header("Spells")]
    public Spell[] spells = new Spell[3];

    public CharacterBaseStats GetStatsAtLevel(int level)
    {
        return new CharacterBaseStats
        {
            hp = hp + (hpPerLevel * (level - 1)),
            atk = atk + (atkPerLevel * (level - 1)),
            spd = spd + (spdPerLevel * (level - 1)),
            def = def + (defPerLevel * (level - 1)),
            critChance = critChance,
            critDamage = critDamage
        };
    }
}
