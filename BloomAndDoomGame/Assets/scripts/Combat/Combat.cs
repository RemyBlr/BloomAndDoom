using UnityEngine;

[System.Serializable]
public struct DamageInfo
{
    public float damage;
    public bool isCritical;
    public GameObject attacker;
}

public class Combat : MonoBehaviour
{
    private CharacterStats characterStats;

    void Start() {
        characterStats = GetComponent<CharacterStats>();
    }

    private bool IsCriticalHit() {
        if (characterStats == null) return false;
        
        float critChance = characterStats.GetCritChance();
        float randomValue = Random.Range(0f, 1f);
        
        return randomValue <= critChance;
    }

    public DamageInfo CalculateDamage(float damageMultiplier = 1f) {
        if (characterStats == null) return new DamageInfo { damage = 0, isCritical = false };
        
        float baseDamage = characterStats.GetAttack() * damageMultiplier;
        bool isCritical = IsCriticalHit();
        float finalDamage = baseDamage;
        
        if (isCritical) {
            float critMultiplier = 1f + characterStats.GetCritDamage();
            finalDamage = baseDamage * critMultiplier;
        }
        
        return new DamageInfo {damage = finalDamage, isCritical = isCritical, attacker = this.gameObject};
    }

    public float DealDamageTo(I_Damageable target, float damageMultiplier = 1f)
    {
        if (target == null) return 0f;
        
        DamageInfo damageInfo = CalculateDamage(damageMultiplier);
        
        // Inflict damage
        target.TakeDamage(damageInfo.damage);

        // Add damage for end screen
        if (gameObject.CompareTag("Player") && GameStats.Instance != null)
            GameStats.Instance.AddDamageDealt(damageInfo.damage);
        
        return damageInfo.damage;
    }
}
