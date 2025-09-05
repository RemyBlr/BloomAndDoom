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
    public Image hp;
    public Image mana;
    public Image xp;

    private float elapsedTime;
    private CharacterStats playerStats;

    void Start()
    {
        SetupPlayerStats();
    }

    void Update()
    {
        // timer
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        time.text = $"Temps: {minutes:00}:{seconds:00}";

        //money.text = characterStats.GetCurrency().ToString();
    }

    public void SetMoney(int amount) {
        money.text = $"Argent: {amount}";
    }

    // Par la suite : hudManager.SetHealth(75, 100);
    public void SetHealth(float current, float max) {
        hp.fillAmount = Mathf.Clamp01(current / max);
    }

    public void SetMana(float current, float max) {
        mana.fillAmount = Mathf.Clamp01(current / max);
    }

    public void SetXp(float current, float max) {
        xp.fillAmount = Mathf.Clamp01(current / max);
    }

    public void SetSpell(int index, Sprite icon, float cooldown) {
        if (index < icons.Length) {
            icons[index].sprite = icon;
            cds[index].text = cooldown > 0 ? $"{cooldown:0.0}s" : "";
        }
    }

    // Par la suite : hudManager.AddObjective("Tuer le boss");
    public void AddObjective(string text) {
        var obj = Instantiate(objectivePrefab, objectivesParent);
        obj.GetComponent<TextMeshProUGUI>().text = text;
    }

    private void SetupPlayerStats() {
        if (GameManager.Instance?.playerInstance != null) {
            playerStats = GameManager.Instance.playerInstance.GetComponent<CharacterStats>();
            if (playerStats == null)
                playerStats = GameManager.Instance.playerInstance.GetComponentInChildren<CharacterStats>();
        }

        if (playerStats != null) {
            CharacterStats.OnCurrencyChanged += UpdateMoneyDisplay;
            UpdateMoneyDisplay(playerStats.GetCurrency());
            
            Debug.Log("HUD connecté aux stats du joueur");
        }
        else
        {
            Debug.LogWarning("CharacterStats du joueur introuvable pour le HUD");
        }
    }

    private void UpdateMoneyDisplay(int newAmount) {
        if (money != null)
            money.text = $"Argent: {newAmount}";
    }

    void OnDestroy() {
        CharacterStats.OnCurrencyChanged -= UpdateMoneyDisplay;
    }
}
