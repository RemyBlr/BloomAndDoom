using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private float speed;

    private float lifetime; //if projectile lasts longer than this, it will be destroyed

    private float m_Damage;

    private string m_TargetTag="Player";
    
    private Rigidbody m_Rigidbody;

    private DamageInfo damageInfo;
    private bool damageCalculated = false;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    public void Initialize(DamageInfo damage)
    {
        damageInfo = damage;
        damageCalculated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        /*Debug.Log("Projectile hit: " + other.name);
        if (other.CompareTag(m_TargetTag))
        {
           I_Damageable victimHealth = other.GetComponent<I_Damageable>();
           if (victimHealth != null)
           {
               victimHealth.TakeDamage(m_Damage);
           }

        }
        Destroy(gameObject); */

        I_Damageable target = other.GetComponent<I_Damageable>();
        
        if (target != null && damageCalculated) {

            target.TakeDamage(damageInfo.damage);
            Debug.Log("Je suis ici");
            
            if (damageInfo.isCritical)
                ShowCriticalEffect();
            
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }

    public void Initialize(float damage, float speed, float lifetime, string targetTag) //We can't use constructors so the initialization has to be done this way in unity
    {
        this.m_Damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.m_TargetTag = targetTag;
        m_Rigidbody.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    private void ShowCriticalEffect() {
        // TODO change popup color, and add ! after damage
        Debug.Log("critical hit");
    }

    private void Update()
    {
        // move projectile
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
