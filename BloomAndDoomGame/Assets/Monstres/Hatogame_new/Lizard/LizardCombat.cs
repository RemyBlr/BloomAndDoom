using UnityEngine;

public class LizardCombat : EnemyCombat 
{
    [SerializeField] private Collider[] HitBoxes;
    
    private void EnableHitBoxAt(int index)
    {
        if (HitBoxes == null || HitBoxes.Length >= index) return;
        HitBoxes[index].enabled = true;
    }
    
    private void DisableHitBoxAt(int index)
    {
        if (HitBoxes == null || HitBoxes.Length >= index) return;
        HitBoxes[index].enabled = false;
    }

    public void EnableHitBox1() => EnableHitBoxAt(0);
    public void DisableHitBox1() => DisableHitBoxAt(0);
    
    public void EnableHitBox2() => EnableHitBoxAt(1);
    public void DisableHitBox2() => DisableHitBoxAt(1);
    
    public void EnableHitBoxes()
    {
        for (int i = 0; i < HitBoxes.Length; i++)
        {
            HitBoxes[i].enabled = true;
        }
    }

    public void DisableHitBoxes()
    {
        for (int i = 0; i < HitBoxes.Length; i++)
        {
            HitBoxes[i].enabled = false;
        }
    }

    protected override void Update() { return; }

    public override void StartAttacking(bool attack)
    {
        if (m_IsAttacking == attack) return;
        base.StartAttacking(attack);
        m_Animator.SetBool("IsAttacking", attack);
    }
}
