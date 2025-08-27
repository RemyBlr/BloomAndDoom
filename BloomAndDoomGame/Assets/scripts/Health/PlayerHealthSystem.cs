using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : BaseHealthSystem {

    private string gameOverScene = "EndScene";
    private CharacterStats characterStats;

    private CharacterStats GetCharacterStats()
    {
        if (characterStats == null)
            characterStats = GetComponent<CharacterStats>();

        return characterStats;
    }
    
    void Start() {
       
        showDamagePopup = false;
    }

    public override void TakeDamage(float damage) {
        CharacterStats stats = GetCharacterStats();
        if (stats == null) {
            Debug.LogWarning("CharacterStats pas encore disponible");
            return;
        }
        
        stats.TakeDamage(damage);
        Debug.Log($"Play prend {damage} dégats");
                
        OnPlayerDamageTaken(damage);

        if (GameStats.Instance != null)
            GameStats.Instance.AddDamageTaken(damage);
        
        if (IsDead())
            OnDeath();
    }

    public override float GetCurrentHealth() {
        return characterStats != null ? characterStats.GetCurrentHealth() : 0f;
    }
    
    public override float GetMaxHealth() {
        return characterStats != null ? characterStats.GetMaxHealth() : 0f;
    }
    
    public override bool IsDead() {
        return GetCurrentHealth() <= 0f;
    }

    protected override void OnDeath() {
        Debug.Log("Player est mort");
        
        if (GameStats.Instance != null)
            GameStats.Instance.EndSession();
        
        // TODO death animation

        SceneManager.LoadScene(gameOverScene);
    }

    private void OnPlayerDamageTaken(float damage) {
        // Can add vfx/sfx here, blinking screen etc.
        OnDamageTaken(damage);
    }

    public void Heal(float amount) {
        if (characterStats != null)
            characterStats.Heal(amount);
    }
}
