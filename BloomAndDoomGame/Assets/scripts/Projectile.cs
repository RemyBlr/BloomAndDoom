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

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Projectile collision avec {other.name} (tag: '{other.tag}')");
        
        // Vérifier si c'est la bonne cible
        if (other.CompareTag(m_TargetTag))
        {
            // Chercher HealthSystem de plusieurs façons
            HealthSystem victimHealth = FindHealthSystem(other);
            
            if (victimHealth != null)
            {
                Debug.Log($"HealthSystem trouvé! Infligeant {m_Damage} dégâts à {other.name}");
                victimHealth.TakeDamage(m_Damage);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning($"HealthSystem introuvable sur {other.name} ou sa hiérarchie!");
            }
        }
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Projectile touche le terrain");
            Destroy(gameObject);
        }
    }

    private HealthSystem FindHealthSystem(Collider collider)
    {
        // 1. Essayer sur l'objet directement
        HealthSystem health = collider.GetComponent<HealthSystem>();
        if (health != null)
        {
            Debug.Log("HealthSystem trouvé sur l'objet principal");
            return health;
        }

        // 2. Essayer sur le parent
        health = collider.GetComponentInParent<HealthSystem>();
        if (health != null)
        {
            Debug.Log($"HealthSystem trouvé sur le parent: {health.gameObject.name}");
            return health;
        }

        // 3. Essayer dans les enfants
        health = collider.GetComponentInChildren<HealthSystem>();
        if (health != null)
        {
            Debug.Log($"HealthSystem trouvé dans les enfants: {health.gameObject.name}");
            return health;
        }

        // 4. Dernier recours : chercher sur le GameObject racine
        Transform root = collider.transform;
        while (root.parent != null)
            root = root.parent;
        
        health = root.GetComponentInChildren<HealthSystem>();
        if (health != null)
        {
            Debug.Log($"HealthSystem trouvé sur la racine: {health.gameObject.name}");
            return health;
        }

        return null;
    }

    public void Initialize(float damage, float speed, float lifetime, string targetTag)
    {
        this.m_Damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.m_TargetTag = targetTag;
        
        Debug.Log($"Projectile initialisé - Dégâts: {damage}, Cible: '{targetTag}'");
        
        if (m_Rigidbody != null)
            m_Rigidbody.linearVelocity = transform.forward * speed;
        
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}