using System;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            player.TakeDamage(m_Damage);
            // Détruire le projectile après avoir infligé des dégâts
            Destroy(gameObject);
        }
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