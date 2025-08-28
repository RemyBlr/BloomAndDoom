using UnityEngine;
using System.Collections.Generic;

public class DealMeleeDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage = 10f;

    [SerializeField]
    private string m_TargetTag = "Player";
    
    [Header("Hit Prevention")]
    [SerializeField] private float hitCooldown = 0.5f;
    
    private Dictionary<GameObject, float> lastHitTimes = new Dictionary<GameObject, float>();
    private EnemyHealthSystem enemyHealth;
    private bool hasHitThisSwing = false; // Pour éviter multiples hits par swing

    void Start()
    {
        // Essayer de trouver les stats de l'ennemi
        enemyHealth = GetComponentInParent<EnemyHealthSystem>();
        if (enemyHealth == null)
        {
            Debug.Log($"EnemyHealthSystem non trouvé sur {gameObject.name}, utilisant dégâts par défaut: {m_Damage}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Éviter les multiples hits pendant le même swing
        if (hasHitThisSwing)
        {
            return;
        }

        if (other.CompareTag(m_TargetTag))
        {
            // Vérifier le cooldown pour éviter les hits trop fréquents
            if (lastHitTimes.ContainsKey(other.gameObject))
            {
                float timeSinceLastHit = Time.time - lastHitTimes[other.gameObject];
                if (timeSinceLastHit < hitCooldown)
                {
                    return; // Trop tôt pour re-hit
                }
            }

            // Chercher le système de santé (amélioration de votre code existant)
            I_Damageable victimHealth = FindHealthSystem(other);
            
            if (victimHealth != null)
            {
                float actualDamage = GetActualDamage();
                
                Debug.Log($"Ennemi {transform.parent?.name ?? gameObject.name} inflige {actualDamage} dégâts de mêlée à {other.name}");
                
                victimHealth.TakeDamage(actualDamage);
                
                // Enregistrer le hit
                lastHitTimes[other.gameObject] = Time.time;
                hasHitThisSwing = true;
            }
            else
            {
                Debug.LogWarning($"Aucun I_Damageable trouvé sur {other.name}");
            }
        }
    }

    private I_Damageable FindHealthSystem(Collider collider)
    {
        // Votre logique originale d'abord
        I_Damageable target = collider.GetComponent<I_Damageable>();
        if (target != null) return target;

        // Puis chercher sur le parent si pas trouvé
        target = collider.GetComponentInParent<I_Damageable>();
        if (target != null) return target;

        // Enfin dans les enfants
        target = collider.GetComponentInChildren<I_Damageable>();
        return target;
    }

    private float GetActualDamage()
    {
        // Utiliser les stats de l'ennemi si disponibles, sinon valeur par défaut
        if (enemyHealth != null)
        {
            return enemyHealth.GetAttackDamage();
        }
        
        return m_Damage;
    }

    // Méthodes appelées par les animations pour contrôler les dégâts
    public void EnableMeleeDamage()
    {
        GetComponent<Collider>().enabled = true;
        hasHitThisSwing = false; // Reset pour permettre un nouveau hit
        Debug.Log($"Dégâts de mêlée activés sur {gameObject.name}");
    }

    public void DisableMeleeDamage()
    {
        GetComponent<Collider>().enabled = false;
        Debug.Log($"Dégâts de mêlée désactivés sur {gameObject.name}");
    }

    // Nettoyage des références détruites
    void Update()
    {
        var keysToRemove = new List<GameObject>();
        foreach (var kvp in lastHitTimes)
        {
            if (kvp.Key == null)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            lastHitTimes.Remove(key);
        }
    }
}