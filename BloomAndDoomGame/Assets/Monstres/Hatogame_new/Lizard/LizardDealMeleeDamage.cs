using System;
using System.Collections.Generic;
using UnityEngine;

public class LizardDealMeleeDamage : MonoBehaviour
{
    [SerializeField] private float Damage = 10f;
    [SerializeField] private string m_TargetTag = "Player";

    private void Awake()
    {
        EnemyDamageSystem stats = GetComponentInParent<EnemyDamageSystem>();
        if (stats == null) return;
        Damage = stats.Stats.attack;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            print($"hit for {Damage}");
            player.TakeDamage(Damage);
        }
    }
}
