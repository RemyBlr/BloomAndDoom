using UnityEngine;

public class EnemyMeleeCombat : EnemyCombat 
{
    protected override void BasicAttack()
    {
        if (m_Animator != null)
        {
            m_Animator.SetTrigger("BasicAttack");
        }
    }
}
