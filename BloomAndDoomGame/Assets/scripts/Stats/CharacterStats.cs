using UnityEngine;
using System;

[System.Serializable]
public class StatModifier {
    public float baseValue;
    public float flatBonus;
    public float percentageBonus;

    public float GetValue() {
        // TODO adapt, when testing is possible
        return (baseValue + flatBonus) * percentageBonus; // maybe (1f + percentageBonus) idk
    }
}

public enum StatType {
    Health,
    Attack,
    Speed,
    Defense,
    CritChance,
    CritDamage
}

//-------------------------------------------------------------------------------------
// This class manages the base stats for the character, stats are coming from the class
// This class also manages experience gain, aswell as levels and currency
// There are 2 types of stats, flat (+10 health on level up), or percentage (unused for now)
// As we manage health in here, there are methods that can be used in HealthSystem
//-------------------------------------------------------------------------------------
public class CharacterStats : MonoBehaviour
{
    [Header("Base")]
    private CharacterClass characterClass;

    [Header("Dynamic")]
    public StatModifier health = new StatModifier();
    public StatModifier attack = new StatModifier();
    public StatModifier speed = new StatModifier();
    public StatModifier defence = new StatModifier();
    public StatModifier critChance = new StatModifier();
    public StatModifier critDamage = new StatModifier();
    
    [Header("Current")]
    [SerializeField]
    private float currentHealth;
    private int currentLevel = 1;
    private int currentXp = 0;
    private int xpForNextLevel = 100;
    private int currency = 0;

    //---------------- Notifiers ----------------
    // Allows multiple scripts to listen to same event
    // UI is notified each time stat has changed
    public static event Action<CharacterStats> OnStatChanged;
    public static event Action<int> OnLevelUp;
    public static event Action<int> OnCurrencyChanged;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (characterClass == null) return;
        
        // Base stats from class
        health.baseValue = characterClass.hp;
        attack.baseValue = characterClass.atk;
        speed.baseValue = characterClass.spd;
        
        // Current health
        currentHealth = health.GetValue();
        
        OnStatChanged?.Invoke(this);
    }

    //---------------- Getters ----------------
    public float GetMaxHealth() => health.GetValue();
    public float GetAttack() => attack.GetValue();
    public float GetSpeed() => speed.GetValue();
    public float GetDefense() => defence.GetValue();
    public float GetCritChance() => Mathf.Clamp01(critChance.GetValue());
    public float GetCritDamage() => critDamage.GetValue();
    
    public float GetCurrentHealth() => currentHealth;
    public int GetLevel() => currentLevel;
    public int GetCurrency() => currency;
    public CharacterClass GetCharacterClass() => characterClass;

    //---------------- Stat modifiers ----------------
    private StatModifier GetStatModifier(StatType statType) {
        return statType switch {
            StatType.Health => health,
            StatType.Attack => attack,
            StatType.Speed => speed,
            StatType.Defense => defence,
            StatType.CritChance => critChance,
            StatType.CritDamage => critDamage,
            _ => throw new ArgumentException("Stat pas trouvée")
        };
    }

    public void AddFlatBonus(StatType statType, float value) {
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
        float healthToAdd = 10f; // +10 health per level
        float attackToAdd = 2f; // +2 attack per level
        float defenceToAdd = 1f; // +1 defence per level

        currentXp -= xpForNextLevel;
        currentLevel++;
        xpForNextLevel = Mathf.RoundToInt(xpForNextLevel * nextLevelRatio);

        // Add flat bonus per level up
        AddFlatBonus(StatType.Health, healthToAdd);
        AddFlatBonus(StatType.Attack, attackToAdd);
        AddFlatBonus(StatType.Defense, defenceToAdd);
        
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
        // Defence calculation
        float finalDamage = Mathf.Max(1f, damage - GetDefense());
        currentHealth = Mathf.Max(0f, currentHealth - finalDamage);
        
        GameStats.Instance?.AddDamageTaken(finalDamage);
        
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
