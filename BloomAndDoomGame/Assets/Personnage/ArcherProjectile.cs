using System;
using UnityEngine;

public class ArcherProjectile : MonoBehaviour
{
    public float Damage = 0;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out EnemyDamageSystem enemy))
        {
            enemy.TakeDamage(Damage);
        }
    }
    
}
