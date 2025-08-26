using UnityEngine;
using System;

[System.Serializable]
public class StatModifier {
    public float baseValue;
    public float flatBonus;
    public float percentageBonus;

    public float GetValue()
    {
        // TODO adapt, when testing is possible
        return (baseValue + flatBonus) * percentageBonus; // maybe (1f + percentageBonus) idk
    }
}

public enum StatType
{
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
// There are 2 types of stats, flat (+10 hp on level up), or percentage (unused for now)
// As we manage health in here, there are methods that can be used in HealthSystem
//-------------------------------------------------------------------------------------
public class CharacterStats : MonoBehaviour
{
    [Header("Base")]
    private CharacterClass characterClass;

    [Header("Dynamic")]
    public StatModifier hp = new StatModifier();
    public StatModifier atk = new StatModifier();
    public StatModifier spd = new StatModifier();
    public StatModifier def = new StatModifier();
    public StatModifier critChance = new StatModifier();
    public StatModifier critDmg = new StatModifier();
    
    [Header("Current")]
    [SerializeField]
    private float currentHp;
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
    void Start()
    {
        if (characterClass == null) return;
        
        // Base stats from class
        hp.baseValue = characterClass.hp;
        atk.baseValue = characterClass.atk;
        spd.baseValue = characterClass.spd;
        
        // Current health
        currentHp = hp.GetValue();
        
        OnStatChanged?.Invoke(this);
    }

    //---------------- Getters ----------------
    public float GetMaxHealth() => hp.GetValue();
    public float GetAttack() => atk.GetValue();
    public float GetSpeed() => spd.GetValue();
    public float GetDefense() => def.GetValue();
    public float GetCritChance() => Mathf.Clamp01(critChance.GetValue());
    public float GetCritDamage() => critDmg.GetValue();
    
    public float GetCurrentHealth() => currentHp;
    public int GetLevel() => currentLevel;
    public int GetCurrency() => currency;
    public CharacterClass GetCharacterClass() => characterClass;

    //---------------- Stat modifiers ----------------
    private StatModifier GetStatModifier(StatType statType)
    {
        return statType switch
        {
            StatType.Health => hp,
            StatType.Attack => atk,
            StatType.Speed => spd,
            StatType.Defense => def,
            StatType.CritChance => critChance,
            StatType.CritDamage => critDmg,
            _ => throw new ArgumentException("Stat pas trouvée")
        };
    }

    public void AddFlatBonus(StatType statType, float value)
    {
        GetStatModifier(statType).flatBonus += value;
        OnStatChanged?.Invoke(this);
    }

    public void AddPercentageBonus(StatType statType, float percentage)
    {
        GetStatModifier(statType).percentageBonus += percentage;
        OnStatChanged?.Invoke(this);
    }

    //---------------- Xp gain ----------------
    private void LevelUp()
    {
        float nextLevelRatio = 1.2f; // 20% more xp per level
        float hpToAdd = 10f;
        float atkToAdd = 2f;
        float defToAdd = 1f;

        currentXp -= xpForNextLevel;
        currentLevel++;
        xpForNextLevel = Mathf.RoundToInt(xpForNextLevel * nextLevelRatio);

        // Add flat bonus per level up
        AddFlatBonus(StatType.Health, hpToAdd);
        AddFlatBonus(StatType.Attack, atkToAdd);
        AddFlatBonus(StatType.Defense, defToAdd);
        
        // Full health on level up ?
        // currentHp = GetMaxHealth();
        
        OnLevelUp?.Invoke(currentLevel);
        OnStatChanged?.Invoke(this);
    }

    public void AddExperience(int xp)
    {
        currentXp += xp;
        GameStats.Instance?.AddExperienceGained(xp);
        
        while (currentXp >= xpForNextLevel)
            LevelUp();
    }

    //---------------- Currency ----------------
    public void AddCurrency(int amount)
    {
        currency += amount;
        GameStats.Instance?.AddCurrencyGained(amount);
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool CanSpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            GameStats.Instance?.AddCurrencySpent(amount);
            OnCurrencyChanged?.Invoke(currency);
            return true;
        }
        return false;
    }

    //---------------- Interface used in HealthSystem ----------------
    public void TakeDamage(float damage)
    {
        // Defence calculation
        float finalDamage = Mathf.Max(1f, damage - GetDefense());
        currentHp = Mathf.Max(0f, currentHp - finalDamage);
        
        GameStats.Instance?.AddDamageTaken(finalDamage);
        
        if (currentHp <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Min(GetMaxHealth(), currentHp + amount);
    }

    private void Die()
    {
        Debug.Log("Player died!");
    }
}
