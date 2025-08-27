using UnityEngine;

public class EnemyHealthSystem : BaseHealthSystem
{
    [Header("Enemy Configuration")]
    [SerializeField] private EnemyStats enemyStats;
    
    private EnemyRuntimeStats runtimeStats;
    private float currentHealth;
    
    void Start() {
        InitializeFromEnemyStats();
        showDamagePopup = true;
    }
    
    private void InitializeFromEnemyStats() {
        if (enemyStats == null) return;
        
        runtimeStats = enemyStats.GetRandomizedStats();
        currentHealth = runtimeStats.maxHealth;
    }

    public override void TakeDamage(float damage) {
        float finalDamage = Mathf.Max(1f, damage - runtimeStats.defense);
        currentHealth = Mathf.Max(0f, currentHealth - finalDamage);
        
        CreateDamagePopup(finalDamage);
        
        OnDamageTaken(finalDamage);
        
        if (IsDead())
            OnDeath();
    }
    
    public override float GetCurrentHealth() {
        return currentHealth;
    }
    
    public override float GetMaxHealth() {
        return runtimeStats.maxHealth;
    }
    
    public override bool IsDead() {
        return currentHealth <= 0f;
    }
    
    protected override void OnDeath() {

        string enemyName = enemyStats != null ? enemyStats.enemyName : gameObject.name;
        Debug.Log($"Ennemi {enemyName} mort");
        
        GiveRewardsToPlayer();
        
        if (GameStats.Instance != null)
            GameStats.Instance.AddEnemyKilled();
        
        Destroy(gameObject, 0.1f);
    }
    
    private void GiveRewardsToPlayer() {
        if (GameManager.Instance?.playerInstance != null) {

            CharacterStats playerStats = GameManager.Instance.playerInstance.GetComponent<CharacterStats>();
            if (playerStats != null) {
                playerStats.AddExperience(runtimeStats.expReward);
                playerStats.AddCurrency(runtimeStats.currencyReward);
                
                Debug.Log($"Récompenses: {runtimeStats.expReward} xp, {runtimeStats.currencyReward} $");
            }
        }
    }
    
    #region Getters pour les autres systèmes
    public float GetAttackDamage() => runtimeStats.attack;
    public float GetMoveSpeed() => runtimeStats.moveSpeed;
    public float GetDetectionRange() => runtimeStats.detectionRange;
    public float GetAttackRange() => runtimeStats.attackRange;
    public float GetAttackSpeed() => runtimeStats.attackSpeed;
    public EnemyStats GetEnemyStats() => enemyStats;
    public EnemyRuntimeStats GetRuntimeStats() => runtimeStats;
    #endregion

    public void SetEnemyStats(EnemyStats newEnemyStats) {
        enemyStats = newEnemyStats;
        InitializeFromEnemyStats();
    }
}