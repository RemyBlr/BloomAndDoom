using UnityEngine;
using UnityEngine.AI;

public class EnemyInvestigateState : EnemyState
{
    private Animator m_Animator;
    private Vector3 m_InvestigatePosition;
    private float m_InvestigateTimer = 0f;

    [SerializeField]
    private float m_InvestigateDuration = 5f;


    public EnemyInvestigateState(Vector3 investigatePosition, EnemyMovement enemyMovement, EnemyPerception enemyPerception)
        : base(enemyMovement, enemyPerception)
    {
        m_InvestigatePosition = investigatePosition;
        m_NavMeshAgent.SetDestination(m_InvestigatePosition);
        m_Animator = m_EnemyMovement.GetComponent<Animator>();
    }

    public override void UpdateState()
    {
        //if sees target, chases it immediately
        if (m_EnemyPerception.DetectTarget() != null)
        {
            EnterChaseState(m_EnemyPerception.DetectTarget());
            return;
        }

        if (!m_NavMeshAgent.pathPending
            && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)

        {
            //If arrived at investigate position and timer is up, goes back to wandering
            m_InvestigateTimer += Time.deltaTime;
            if (m_InvestigateTimer >= m_InvestigateDuration)
            {
                EnterWanderState();
                return;
            }
        }

    }

    private void EnterWanderState()
    {
        m_Animator.SetBool("IsRunning", false);
        m_EnemyMovement.ChangeState(new EnemyWanderState(m_EnemyMovement, m_EnemyPerception));
    }

    private void EnterChaseState(GameObject target)
    {
        m_EnemyMovement.ChangeState(new EnemyChaseState(target, m_EnemyMovement, m_EnemyPerception));
    }
}
