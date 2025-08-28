using UnityEngine;

public class EnemyProjectileCombat : EnemyCombat 
{
    [Header("Projectile Parameters")]

    [SerializeField]
    private GameObject m_ProjectilePrefab;

    [SerializeField]
    private Transform m_FirePoint;

    [SerializeField]
    private float m_ProjectileSpeed = 10f;

    [SerializeField]
    private float m_ProjectileLifetime = 5f;

    [SerializeField]
    private string m_ProjectileTargetTag = "Player";

    [SerializeField]
    private float m_ProjectileDamage = 10f;

    protected override void BasicAttack()
    {
        if (m_ProjectilePrefab != null && m_FirePoint != null)
        {
            GameObject projectileInstance = Instantiate(m_ProjectilePrefab, m_FirePoint.position, m_FirePoint.rotation);

            if(m_Animator != null)
            {
                m_Animator.SetTrigger("BasicAttack");
            }

            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            if (projectile != null)
            {
                Rigidbody projectileRigidBody = projectileInstance.GetComponent<Rigidbody>();
                if (projectileRigidBody != null)
                {
                    projectile.Initialize(m_ProjectileDamage, m_ProjectileSpeed, m_ProjectileLifetime, m_ProjectileTargetTag);
                }
                else
                {
                    Debug.LogError("Rigidbody component not found on the projectile instance.");
                }
            }
            else
            {
                Debug.LogError("Projectile component not found on the projectile prefab.");
            }
        }
        else
        {
            Debug.LogError("Projectile prefab or fire point is not set.");
        }
    }
}
