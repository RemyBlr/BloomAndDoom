using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Top left")]
    public TextMeshProUGUI time;
    public TextMeshProUGUI money;

    [Header("Top Right")]
    public RawImage minimap;
    public Transform objectivesParent;
    public GameObject objectivePrefab;

    [Header("Bottom Center")]
    public Image[] icons;
    public TextMeshProUGUI[] cds;

    [Header("Bottom Left")]
    public Slider hp;
    public Slider mana;

    private float elapsedTime;

    void Update()
    {
        // timer
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        time.text = $"Temps: {minutes:00}:{seconds:00}";
    }

    public void SetMoney(int amount) {
        money.text = $"Argent: {amount}";
    }

    public void SetHealth(float value) {
        hp.value = value;
    }

    public void SetMana(float value) {
        mana.value = value;
    }

    public void SetSpell(int index, Sprite icon, float cooldown) {
        if (index < icons.Length) {
            icons[index].sprite = icon;
            cds[index].text = cooldown > 0 ? $"{cooldown:0.0}s" : "";
        }
    }

    public void AddObjective(string text) {
        var obj = Instantiate(objectivePrefab, objectivesParent);
        obj.GetComponent<TextMeshProUGUI>().text = text;
    }
}
