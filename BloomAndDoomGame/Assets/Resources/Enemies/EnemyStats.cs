using UnityEngine;

[System.Serializable]
public struct EnemyRuntimeStats
{
    public float maxHealth;
    public float attack;
    public float defense;
    public float moveSpeed;
    public int expReward;
    public int currencyReward;
    public float detectionRange;
    public float attackRange;
    public float attackSpeed;
}

[CreateAssetMenu(fileName="NewEnemy", menuName="Enemy/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Basic")]
    public string enemyName;
    
    [Header("Combat Stats")]
    public float maxHealth = 50f;
    public float attack = 10f;
    public float defense = 2f;
    public float moveSpeed = 3f;
    
    [Header("Rewards")]
    public int expReward = 15;
    public int currencyReward = 10;
    
    [Header("Behavior")]
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float attackSpeed = 1f;
    public int spawnAtLevel = 1;
    
    [Header("Visual")]
    public GameObject prefab;
    
    public EnemyRuntimeStats GetRandomizedStats()
    {
        return new EnemyRuntimeStats
        {
            maxHealth = maxHealth * Random.Range(0.8f, 1.2f),
            attack = attack * Random.Range(0.9f, 1.1f),
            defense = defense * Random.Range(0.8f, 1.2f),
            moveSpeed = moveSpeed * Random.Range(0.9f, 1.1f),
            expReward = Mathf.RoundToInt(expReward * Random.Range(0.8f, 1.2f)),
            currencyReward = Mathf.RoundToInt(currencyReward * Random.Range(0.8f, 1.2f)),
            detectionRange = detectionRange,
            attackRange = attackRange,
            attackSpeed = attackSpeed
        };
    }
}