
using UnityEngine.AI;

public  abstract class EnemyState
{
    protected EnemyMovement m_EnemyMovement;
    protected EnemyPerception m_EnemyPerception;

    protected NavMeshAgent m_NavMeshAgent;

    public EnemyState(EnemyMovement enemyMovement, EnemyPerception enemyPerception)
    {
        m_EnemyMovement = enemyMovement;
        m_EnemyPerception = enemyPerception;
        m_NavMeshAgent = m_EnemyMovement.GetComponent<NavMeshAgent>();
    }
    public abstract void UpdateState();

    public abstract void OnExitState();

    public virtual void OnEnterState()
    {
        return;
    }
}
