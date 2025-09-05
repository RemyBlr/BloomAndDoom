using System;
using System.Collections.Generic;
using UnityEngine;

public class LizardDealMeleeDamage : MonoBehaviour
{
    [SerializeField] private EnemyDamageSystem enemyDamageSystem;
    [SerializeField] private float Damage = 10f;
    [SerializeField] private string m_TargetTag = "Player";

    private void Awake()
    {
        if (enemyDamageSystem == null) return;
        Damage = enemyDamageSystem.Stats.attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            player.TakeDamage(Damage);
        }
    }
}
