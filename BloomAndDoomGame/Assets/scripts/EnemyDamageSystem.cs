using System;
using UnityEngine;

public class EnemyDamageSystem : MonoBehaviour
{
    public EnemyStats Stats;
    private Animator animator;

    public float currentHeal = 100;

    public bool IsDead => currentHeal < 0 || Mathf.Approximately(currentHeal, 0);
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHeal = Stats.maxHealth;
    }

    public void TakeDamage(float value)
    {
        print($"touch");
        if (IsDead) return;
        currentHeal =  currentHeal - CalculateDamage(value);
        if (!IsDead) return;
        animator.SetTrigger("IsDead");
    }

    private float CalculateDamage(float rawDamage)
    {
        return rawDamage - Stats.defense;
    }
}
