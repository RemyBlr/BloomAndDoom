using UnityEngine;

public class EnemyMeleeCombat : EnemyCombat 
{
    [SerializeField]
    private Collider m_WeaponCollider;

    protected override void BasicAttack()
    {
        if (m_Animator != null)
        {
            m_Animator.SetTrigger("BasicAttack");
        }
    }


    //We need to have 2 separate methods for enabling and disabling the weapon collider because we canot set a bool as a parameter in an animation event
    public void EnableWeaponCollider()
    {
        if (m_WeaponCollider != null)
        {
            m_WeaponCollider.enabled = true;
        }
    }

    public void DisableWeaponCollider()
    {
        if (m_WeaponCollider != null)
        {
            m_WeaponCollider.enabled = false;
        }
    }
}
