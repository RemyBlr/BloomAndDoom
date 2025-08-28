using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : BaseHealthSystem {

    private string gameOverScene = "EndScene";
    private CharacterStats characterStats;
    private bool isDead = false; // Flag pour éviter les multiples morts

    private CharacterStats GetCharacterStats()
    {
        if (characterStats == null)
        {
            // Chercher d'abord sur ce GameObject
            characterStats = GetComponent<CharacterStats>();
            
            // Si pas trouvé, chercher sur le parent
            if (characterStats == null)
            {
                characterStats = GetComponentInParent<CharacterStats>();
                if (characterStats != null)
                    Debug.Log("CharacterStats trouvé sur le parent");
            }
            
            // Si pas trouvé, chercher dans les enfants
            if (characterStats == null)
            {
                characterStats = GetComponentInChildren<CharacterStats>();
                if (characterStats != null)
                    Debug.Log("CharacterStats trouvé dans les enfants");
            }
            
            if (characterStats != null)
                Debug.Log($"CharacterStats trouvé sur {characterStats.gameObject.name}");
            else
                Debug.LogError("CharacterStats introuvable!");
        }

        return characterStats;
    }
    
    void Start() {
        showDamagePopup = false;
        
        // Attendre une frame pour que CharacterStats soit initialisé
        Invoke(nameof(InitializeHealth), 0.1f);
    }
    
    void InitializeHealth()
    {
        CharacterStats stats = GetCharacterStats();
        if (stats != null)
        {
            float maxHP = stats.GetMaxHealth();
            Debug.Log($"PlayerHealthSystem initialisé avec {maxHP} HP max");
            
            if (maxHP <= 0)
            {
                Debug.LogWarning("HP max est 0 ou négatif! Vérifiez votre CharacterClass.");
            }
        }
        else
        {
            Debug.LogError("Impossible d'initialiser les HP - CharacterStats manquant!");
        }
    }

    public override void TakeDamage(float damage) {
        // Vérifier si déjà mort pour éviter double traitement
        if (isDead)
        {
            Debug.Log("Joueur déjà mort, ignorant les dégâts supplémentaires");
            return;
        }
        
        CharacterStats stats = GetCharacterStats();
        if (stats == null) {
            Debug.LogWarning("CharacterStats pas encore disponible");
            return;
        }
        
        Debug.Log($"Player prend {damage} dégâts via CharacterStats");
        stats.TakeDamage(damage);
                
        OnPlayerDamageTaken(damage);

        if (GameStats.Instance != null)
            GameStats.Instance.AddDamageTaken(damage);
        
        if (IsDead())
            OnDeath();
    }

    public override float GetCurrentHealth() {
        CharacterStats stats = GetCharacterStats();
        return stats != null ? stats.GetCurrentHealth() : 0f;
    }
    
    public override float GetMaxHealth() {
        CharacterStats stats = GetCharacterStats();
        return stats != null ? stats.GetMaxHealth() : 0f;
    }
    
    public override bool IsDead() {
        return GetCurrentHealth() <= 0f || isDead;
    }

    protected override void OnDeath() {
        if (isDead) return; // Éviter de mourir plusieurs fois
        
        isDead = true;
        Debug.Log("Player est mort");
        
        if (GameStats.Instance != null)
            GameStats.Instance.EndSession();
        
        // TODO death animation
        
        // Délai avant de charger la scène pour éviter les conflits
        Invoke(nameof(LoadGameOverScene), 1f);
    }
    
    void LoadGameOverScene()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    private void OnPlayerDamageTaken(float damage) {
        // Can add vfx/sfx here, blinking screen etc.
        OnDamageTaken(damage);
    }

    public void Heal(float amount) {
        if (isDead) return; // Pas de soin si mort
        
        CharacterStats stats = GetCharacterStats();
        if (stats != null)
            stats.Heal(amount);
    }
    
    // Méthode pour debug
    [ContextMenu("Debug Health Info")]
    void DebugHealthInfo()
    {
        CharacterStats stats = GetCharacterStats();
        if (stats != null)
        {
            Debug.Log($"HP Actuel: {stats.GetCurrentHealth()}");
            Debug.Log($"HP Max: {stats.GetMaxHealth()}");
            Debug.Log($"Est mort: {IsDead()}");
        }
        else
        {
            Debug.Log("CharacterStats non trouvé!");
        }
    }
}