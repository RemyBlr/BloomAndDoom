using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjectileAudio : MonoBehaviour
{
    private float speed;

    private float lifetime; //if projectile lasts longer than this, it will be destroyed

    private float m_Damage;

    private string m_TargetTag="Player";
    
    private Rigidbody m_Rigidbody;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    /*void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name);
        if (other.CompareTag(m_TargetTag))
        {
        //    HealthSystem victimHealth = other.GetComponent<HealthSystem>();
        //    if (victimHealth != null)
        //    {
        //        victimHealth.TakeDamage(m_Damage);
        //    }

        }
        Destroy(gameObject); 
    }*/

    public void Initialize(float damage, float speed, float lifetime, string targetTag) //We can't use constructors so the initialization has to be done this way in unity
    {
        this.m_Damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.m_TargetTag = targetTag;
        m_Rigidbody.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }
}
