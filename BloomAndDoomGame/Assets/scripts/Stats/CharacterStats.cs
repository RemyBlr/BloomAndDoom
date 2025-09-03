using UnityEngine;
using System;

[System.Serializable]
public class StatModifier {
    public float baseValue;
    public float flatBonus;
    public float percentageBonus;

    public StatModifier(float initialBaseValue = 0f) {
        baseValue = initialBaseValue;
        flatBonus = 0f;
        percentageBonus = 0f;
    }

    public float GetValue() {
        return (baseValue + flatBonus) * (1f + percentageBonus);
    }

    public void SetValue(float newValue)
    {
        baseValue = newValue;
    }
}

public enum StatType {
    Health,
    Attack,
    Speed,
    Defense,
    CritChance,
    CritDamage,
    AttackSpeed
}

//-------------------------------------------------------------------------------------
// This class manages the base stats for the character, stats are coming from the class
// This class also manages experience gain, aswell as levels and currency
// There are 2 types of stats, flat (+10 health on level up), or percentage (unused for now)
// As we manage health in here, there are methods that can be used in HealthSystem
//-------------------------------------------------------------------------------------
public class CharacterStats : MonoBehaviour
{
    private PlayerController player;

    [Header("Base")]
    [SerializeField] private CharacterClass characterClass;

    [Header("Dynamic")]
    [SerializeField] private StatModifier health = new StatModifier();
    [SerializeField] private StatModifier attack = new StatModifier();
    [SerializeField] private StatModifier speed = new StatModifier();
    [SerializeField] private StatModifier defense = new StatModifier();
    [SerializeField] private StatModifier critChance = new StatModifier();
    [SerializeField] private StatModifier critDamage = new StatModifier();
    [SerializeField] private StatModifier attackSpeed = new StatModifier();
    
    [Header("Current")]
    [SerializeField] private float currentHealth;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXp = 0;
    [SerializeField] private int xpForNextLevel = 100;
    [SerializeField] private int currency = 0;

    private HUDManager hud;

    //---------------- Notifiers ----------------
    // Allows multiple scripts to listen to same event
    // UI is notified each time stat has changed
    public static event Action<CharacterStats> OnStatChanged;
    public static event Action<int> OnLevelUp;
    public static event Action<int> OnCurrencyChanged;
    
    private void InitializeFromClass() {
        if (characterClass == null) 
        {
            Debug.LogError("CharacterClass est null! Impossible d'initialiser les stats.");
            return;
        }

        Debug.Log($"Initialisation des stats pour la classe: {characterClass.className}");
        
        CharacterBaseStats baseStats = characterClass.GetStatsAtLevel(currentLevel);
        
        // Base stats from class
        health = new StatModifier(baseStats.hp);
        attack = new StatModifier(baseStats.atk);
        speed = new StatModifier(baseStats.spd);
        defense = new StatModifier(baseStats.def);
        critChance = new StatModifier(baseStats.critChance);
        critDamage = new StatModifier(baseStats.critDamage);
        attackSpeed = new StatModifier(baseStats.atkSpd);

        currentLevel = characterClass.startingLevel;
        currency = characterClass.startingCurrency;
        
        // Current health - IMPORTANT: initialiser après avoir défini health
        float maxHP = health.GetValue();
        currentHealth = maxHP;
        
        Debug.Log($"Stats initialisées: HP={maxHP}, ATK={baseStats.atk}, Level={currentLevel}");
        
        OnStatChanged?.Invoke(this);
    }
    
    void Start()
    {
        var spell1 = characterClass.spells[0].icon;

        player = GetComponent<PlayerController>();
        hud = FindFirstObjectByType<HUDManager>();
        InitializeFromClass();
    }

    public void SetCharacterClass(CharacterClass newClass) {
        characterClass = newClass;
        InitializeFromClass();
    }

    //---------------- Getters ----------------
    public float GetMaxHealth() => health.GetValue();
    public float GetAttack() => attack.GetValue();
    public float GetSpeed() => speed.GetValue();
    public float GetDefense() => defense.GetValue();
    public float GetCritChance() => Mathf.Clamp01(critChance.GetValue());
    public float GetCritDamage() => critDamage.GetValue();
    public float GetAttackSpeed() => attackSpeed.GetValue();
    
    public float GetCurrentHealth() => currentHealth;
    public int GetLevel() => currentLevel;
    public int GetCurrency() => currency;
    public CharacterClass GetCharacterClass() => characterClass;

    public void SetHealth(float amount)
    {
        if (amount > GetMaxHealth())
            amount = GetMaxHealth();
        health.SetValue(amount);
    }
    public void SetDefense(float amount) { defense.SetValue(amount); }
    public void SetAttack(float amount) { attack.SetValue(amount); }
    public void SetCritDamage(float amount) { critDamage.SetValue(amount);  }
    public void SetCurrency(int amount) { currency = amount; }

    //---------------- Stat modifiers ----------------
    private StatModifier GetStatModifier(StatType statType)
    {
        return statType switch
        {
            StatType.Health => health,
            StatType.Attack => attack,
            StatType.Speed => speed,
            StatType.Defense => defense,
            StatType.CritChance => critChance,
            StatType.CritDamage => critDamage,
            StatType.AttackSpeed => attackSpeed,
            _ => throw new ArgumentException("Stat pas trouvée")
        };
    }

    public void SetAttackSpeed(float amount)
    {
        attackSpeed.SetValue(amount);
    }

    public void SetSpeed(float amount)
    {
        speed.SetValue(amount);
        player.SetSpeed(amount);
    }

    public void AddFlatBonus(StatType statType, float value)
    {
        GetStatModifier(statType).flatBonus += value;
        OnStatChanged?.Invoke(this);
    }

    public void AddPercentageBonus(StatType statType, float percentage) {
        GetStatModifier(statType).percentageBonus += percentage;
        OnStatChanged?.Invoke(this);
    }

    //---------------- Xp gain ----------------
    private void LevelUp() {
        float nextLevelRatio = 1.2f; // 20% more xp per level

        currentXp -= xpForNextLevel;
        currentLevel++;
        xpForNextLevel = Mathf.RoundToInt(xpForNextLevel * nextLevelRatio);

        // Add flat bonus per level up
        if (characterClass != null) {
            AddFlatBonus(StatType.Health, characterClass.hpPerLevel);
            AddFlatBonus(StatType.Attack, characterClass.atkPerLevel);
            AddFlatBonus(StatType.Defense, characterClass.defPerLevel);
            AddFlatBonus(StatType.Speed, characterClass.spdPerLevel);
        }

        
        // Full health on level up ?
        currentHealth = GetMaxHealth();
        
        OnLevelUp?.Invoke(currentLevel);
        OnStatChanged?.Invoke(this);
    }

    public void AddExperience(int xp) {
        currentXp += xp;
        GameStats.Instance?.AddExperienceGained(xp);
        
        while (currentXp >= xpForNextLevel)
            LevelUp();
    }

    //---------------- Currency ----------------
    public void AddCurrency(int amount) {
        currency += amount;
        GameStats.Instance?.AddCurrencyGained(amount);
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool CanSpendCurrency(int amount) {
        if (currency >= amount) {
            currency -= amount;
            GameStats.Instance?.AddCurrencySpent(amount);
            OnCurrencyChanged?.Invoke(currency);
            return true;
        }
        return false;
    }

    //---------------- Interface used in HealthSystem ----------------
    public void TakeDamage(float damage) {
        // defense calculation
        float finalDamage = Mathf.Max(1f, damage - GetDefense());
        currentHealth = Mathf.Max(0f, currentHealth - finalDamage);
        
        GameStats.Instance?.AddDamageTaken(finalDamage);
        hud.SetHealth(currentHealth, GetMaxHealth());
        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Min(GetMaxHealth(), currentHealth + amount);
    }

    private void Die() {
        Debug.Log("Player died!");
    }
}
