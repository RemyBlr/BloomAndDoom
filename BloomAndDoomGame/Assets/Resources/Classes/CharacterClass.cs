using UnityEngine;
using UnityEngine.UI;


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

    [System.Serializable]
    public class Spell {
        public Sprite icon;
        public string description;
    }

    [Header("Spells")]
    public Spell[] spells = new Spell[3];
}
