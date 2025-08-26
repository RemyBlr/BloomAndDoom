using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyState
{
    private GameObject m_TargetObject;

    [SerializeField]
    private float m_WanderRadiusMin = 10f;
    [SerializeField]
    private float m_WanderRadiusMax = 15f;
    private Vector3 m_WanderTarget;

    private bool m_ArrivedAtTarget = true;

    private NavMeshAgent m_NavMeshAgent;

    [Tooltip("How quickly the enemy becomes alert to the player")]
    [SerializeField]
    private float m_TargetCheckInterval = 1f;
    private float m_CheckTimer = 0f;

    public EnemyWanderState(EnemyMovement enemyMovement, EnemyPerception enemyPerception)
        : base(enemyMovement, enemyPerception)
    {
        m_NavMeshAgent = m_EnemyMovement.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        Debug.Log("Entering Wander State");
    }

    public override void UpdateState()
    {
        // Logic for updating the wander state

         m_CheckTimer += Time.deltaTime;
         if (m_CheckTimer >= m_TargetCheckInterval)
         {
             LookForTarget();
             m_CheckTimer = 0f;
         }

        if (m_ArrivedAtTarget)
        {
            m_WanderTarget = GetRandomWanderPoint();
            m_NavMeshAgent.SetDestination(m_WanderTarget);
            m_ArrivedAtTarget = false;
            Debug.Log("New wander target: " + m_WanderTarget);
            m_NavMeshAgent.isStopped = false;
        }else
        {
            if (!m_NavMeshAgent.pathPending
                && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
            {
                m_ArrivedAtTarget = true;
            }
        }

    }

    public override void ExitState()
    {
        // Logic for exiting the wander state
    }


    private Vector3 GetRandomWanderPoint()
    {
        NavMeshHit navHit;
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(m_WanderRadiusMin, m_WanderRadiusMax);
            randomDirection += m_EnemyMovement.transform.position;
            NavMesh.SamplePosition(randomDirection, out navHit, m_WanderRadiusMax, -1);
        } while (navHit.position == Vector3.zero);
        return navHit.position;
    }

    private void LookForTarget()
    {
        m_TargetObject = m_EnemyPerception.DetectTarget();
        if (m_TargetObject != null)
        {
            m_EnemyMovement.ChangeState(new EnemyChaseState(m_TargetObject, m_EnemyMovement, m_EnemyPerception));
            return;
        }
    }
}
