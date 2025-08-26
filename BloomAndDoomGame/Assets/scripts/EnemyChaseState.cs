using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    GameObject m_TargetObject;
    private Vector3 m_TargetLastPosition;

    [SerializeField]
    private float m_AttackRange = 0.5f;
    [SerializeField]
    private float m_RotationSpeed = 5f;

    [Tooltip("Angle within which the enemy can attack the target")]
    [SerializeField]
    private float m_AttackingAngle = 45f;

    [Tooltip("Duration the enemy will continue to chase after losing sight of the target")]
    [SerializeField]
    private float m_TimeToLoseTarget = 5f;
    private float m_LoseTargetTimer = 0f;

    private NavMeshAgent m_NavMeshAgent;
    private EnemyCombat m_EnemyCombat;

    public EnemyChaseState(GameObject target, EnemyMovement enemyMovement, EnemyPerception enemyPerception)
        : base(enemyMovement, enemyPerception)
    {
        m_TargetObject = target;
        m_NavMeshAgent = m_EnemyMovement.GetComponent<NavMeshAgent>();
        m_EnemyCombat = m_EnemyMovement.GetComponent<EnemyCombat>();
    }

    public override void EnterState()
    {
        Debug.Log("Entering Chase State");
    }

    public override void UpdateState()
    {
            CheckStateValidity();

            float distanceToTarget = Vector3.Distance(m_EnemyMovement.transform.position, m_TargetLastPosition);

            //turn towards target
            Vector3 direction = (m_TargetLastPosition - m_EnemyMovement.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            m_EnemyMovement.transform.rotation = Quaternion.Slerp(m_EnemyMovement.transform.rotation, lookRotation, Time.deltaTime * m_RotationSpeed);
            m_NavMeshAgent.SetDestination(m_TargetLastPosition);
            m_NavMeshAgent.isStopped = false;

            if (m_EnemyCombat != null)
            {

                if (distanceToTarget <= m_AttackRange && Vector3.Angle(direction, m_EnemyMovement.transform.forward) < m_AttackingAngle)
                {
                    {
                        m_EnemyCombat.StartAttacking(true);
                    }
                }
                else
                {
                    m_EnemyCombat.StartAttacking(false);
                }
            }
    }

    public override void ExitState()
    {
            if (m_EnemyCombat != null)
            {
                m_EnemyCombat.StartAttacking(false);
            }
            m_NavMeshAgent.isStopped = true;
            m_LoseTargetTimer = 0f; // Reset timer
            m_TargetObject = null;
            m_TargetLastPosition = Vector3.zero;
        Debug.Log("Exiting Chase State");
    }

    private void CheckStateValidity()
    {
        if (m_EnemyPerception.DetectTarget() != null)
        {
            m_TargetLastPosition = m_TargetObject.transform.position;
            m_LoseTargetTimer = 0f; // Reset timer if target is visible
        }
        else
        {
            m_LoseTargetTimer += Time.deltaTime;
        }

        if (m_LoseTargetTimer >= m_TimeToLoseTarget)
        {
            EnterInvestigateState();
            return;
        }
    }

    private void EnterInvestigateState()
    {
        m_EnemyMovement.ChangeState(new EnemyInvestigateState(m_TargetLastPosition, m_EnemyMovement, m_EnemyPerception));
    }

    
}
