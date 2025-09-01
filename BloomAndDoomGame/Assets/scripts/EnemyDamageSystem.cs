using System;
using UnityEngine;

public class EnemyDamageSystem : MonoBehaviour
{
    public EnemyStats Stats;

    private float currentHeal = 100;

    public bool IsDead => currentHeal < 0 || Mathf.Approximately(currentHeal, 0);
    
    private void Awake()
    {
        currentHeal = Stats.maxHealth;
    }

    public void TakeDamage(float value)
    {
        currentHeal -= CalculateDamage(value);
    }

    private float CalculateDamage(float rawDamage)
    {
        return rawDamage - Stats.defense;
    }
}
