using UnityEngine;

public class EnemyDeadState : EnemyState
{
    private Animator animator;
    
    public EnemyDeadState(EnemyMovement enemyMovement, EnemyPerception enemyPerception) : base(enemyMovement, enemyPerception)
    {
        animator = enemyMovement.GetComponent<Animator>();
    }

    public override void UpdateState() { return; }

    public override void OnEnterState()
    {
        animator.SetTrigger("IsDead");
    }

    public override void OnExitState()
    {
        return;
    }
}
