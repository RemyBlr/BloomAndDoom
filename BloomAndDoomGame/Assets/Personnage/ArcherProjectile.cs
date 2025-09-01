using System;
using UnityEngine;

public class ArcherProjectile : MonoBehaviour
{
    public float Damage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyDamageSystem enemy))
        {
            enemy.TakeDamage(Damage);
        }
    }
}
