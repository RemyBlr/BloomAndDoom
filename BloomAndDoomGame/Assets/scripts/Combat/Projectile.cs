using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private float m_Damage;
    private string m_TargetTag = "Player";
    private Rigidbody m_Rigidbody;
    private DamageInfo damageInfo;
    private bool damageCalculated = false;
    private bool hasHitTarget = false; // Flag pour éviter multiples hits

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    public void Initialize(DamageInfo damage)
    {
        damageInfo = damage;
        damageCalculated = true;
        Debug.Log($"Projectile initialisé avec DamageInfo - Dégâts: {damage.damage}");
    }

    public void Initialize(float damage, float speed, float lifetime, string targetTag)
    {
        this.m_Damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.m_TargetTag = targetTag;
        
        m_Rigidbody.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
        
        Debug.Log($"Projectile initialisé - Dégâts: {damage}, Cible: '{targetTag}'");
    }

    void OnTriggerEnter(Collider other)
    {
        // Éviter les multiples collisions
        if (hasHitTarget)
        {
            Debug.Log("Projectile a déjà touché une cible, ignorant");
            return;
        }

        Debug.Log($"Projectile collision avec {other.name} (tag: '{other.tag}')");

        // Vérifier si c'est la bonne cible
        if (!other.CompareTag(m_TargetTag))
        {
            Debug.Log($"Tag incorrect: '{other.tag}' != '{m_TargetTag}'");
            
            // Détruire le projectile s'il touche le sol
            if (other.CompareTag("Ground") || other.name.Contains("Ground") || other.name.Contains("Terrain"))
            {
                Debug.Log("Projectile touche le sol");
                Destroy(gameObject);
            }
            return;
        }

        // Chercher le système de dégâts
        I_Damageable target = FindHealthSystem(other);
        
        if (target != null)
        {
            hasHitTarget = true; // Marquer comme ayant touché
            
            float damageToInflict = damageCalculated ? damageInfo.damage : m_Damage;
            
            Debug.Log($"I_Damageable trouvé! Infligeant {damageToInflict} dégâts à {other.name}");
            
            target.TakeDamage(damageToInflict);
            
            if (damageCalculated && damageInfo.isCritical)
                ShowCriticalEffect();
            
            // Détruire le projectile après avoir infligé des dégâts
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning($"Aucun I_Damageable trouvé sur {other.name}");
        }
    }

    private I_Damageable FindHealthSystem(Collider collider)
    {
        // Chercher sur l'objet touché
        I_Damageable target = collider.GetComponent<I_Damageable>();
        if (target != null)
        {
            Debug.Log("I_Damageable trouvé sur l'objet touché");
            return target;
        }

        // Chercher sur l'objet principal (parent)
        target = collider.GetComponentInParent<I_Damageable>();
        if (target != null)
        {
            Debug.Log("I_Damageable trouvé sur l'objet principal");
            return target;
        }

        // Chercher dans les enfants
        target = collider.GetComponentInChildren<I_Damageable>();
        if (target != null)
        {
            Debug.Log("I_Damageable trouvé dans les enfants");
            return target;
        }

        return null;
    }

    private void ShowCriticalEffect()
    {
        Debug.Log("Critical hit effect!");
        // TODO: Ajouter des effets visuels pour les critiques
    }

    private void Update()
    {
        // Mouvement du projectile (si pas de Rigidbody velocity)
        if (m_Rigidbody.linearVelocity.magnitude < 0.1f)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}