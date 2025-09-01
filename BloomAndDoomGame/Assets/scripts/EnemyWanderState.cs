using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyState
{
    private GameObject m_TargetObject;

    private Animator m_Animator;
    private bool m_IsWalking = false;

    [SerializeField]
    private float m_WanderRadiusMin = 10f;
    [SerializeField]
    private float m_WanderRadiusMax = 15f;
    private Vector3 m_WanderTarget;

    private bool m_ArrivedAtTarget = true;


    [Tooltip("How quickly the enemy becomes alert to the player")]
    [SerializeField]
    private float m_TargetCheckInterval = 1f;
    private float m_CheckTimer = 0f;

    public EnemyWanderState(EnemyMovement enemyMovement, EnemyPerception enemyPerception)
        : base(enemyMovement, enemyPerception)
    {
        m_NavMeshAgent.speed = enemyMovement.GetWanderingSpeed();
        m_Animator = m_EnemyMovement.GetComponent<Animator>();
        if (m_EnemyPerception == null)
        {
            Debug.LogError("EnemyPerception component not found on " + m_EnemyMovement.gameObject.name);
        }
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
            m_NavMeshAgent.isStopped = false;
            if (!m_IsWalking && m_Animator != null)
            {
                m_Animator.SetBool("IsWalking", true);
                m_IsWalking = true; // Set the flag to true after starting to walk
            }
        }
        else
        {
            if (!m_NavMeshAgent.pathPending
                && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
            {
                m_ArrivedAtTarget = true;
                m_NavMeshAgent.isStopped = true;
                if (m_Animator != null)
                {
                    m_Animator.SetBool("IsWalking", false);
                    m_IsWalking = false;
                }
            }
        }

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
