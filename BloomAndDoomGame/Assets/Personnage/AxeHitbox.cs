using System;
using UnityEngine;

public class AxeHitbox : MonoBehaviour
{
          public float Damage = 10;

          private void OnTriggerEnter(Collider other)
          {
                    if (other.TryGetComponent(out EnemyDamageSystem enemy))
                    {
                              enemy.TakeDamage(Damage);
                    }
          }
}