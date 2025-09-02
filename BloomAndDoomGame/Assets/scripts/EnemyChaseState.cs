using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    GameObject m_TargetObject;
    private Vector3 m_TargetLastPosition;

    [SerializeField]
    private float m_AttackRange;
    [SerializeField]
    private float m_RotationSpeed = 5f;

    [Tooltip("Angle within which the enemy can attack the target")]
    [SerializeField]
    private float m_AttackingAngle = 45f;

    [Tooltip("Duration the enemy will continue to chase after losing sight of the target")]
    [SerializeField]
    private float m_TimeToLoseTarget = 5f;
    private float m_LoseTargetTimer = 0f;

    private EnemyCombat m_EnemyCombat;
    private Animator m_Animator;

    private bool m_IsRunning = false;

    public EnemyChaseState(GameObject target, EnemyMovement enemyMovement, EnemyPerception enemyPerception)
        : base(enemyMovement, enemyPerception)
    {
        m_TargetObject = target;
        m_NavMeshAgent.speed = enemyMovement.GetChasingSpeed();
        m_EnemyCombat = m_EnemyMovement.GetComponent<EnemyCombat>();
        if(m_EnemyCombat == null)
        {
            Debug.LogError("EnemyCombat component not found on " + m_EnemyMovement.gameObject.name);
        }
        m_Animator = m_EnemyMovement.GetComponent<Animator>();
        m_AttackRange = m_EnemyCombat.GetAttackRange();
    }

    public override void UpdateState()
    {
        CheckStateValidity();

        float distanceToTarget = Vector3.Distance(m_EnemyMovement.transform.position, m_TargetLastPosition);

        //turn towards target
        Vector3 direction = (m_TargetLastPosition - m_EnemyMovement.transform.position).normalized;
        if (new Vector2(direction.x, direction.z) != Vector2.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            m_EnemyMovement.transform.rotation = Quaternion.Slerp(m_EnemyMovement.transform.rotation, lookRotation, Time.deltaTime * m_RotationSpeed);
        }
        m_NavMeshAgent.SetDestination(m_TargetLastPosition);
        m_NavMeshAgent.isStopped = false;

        if (!m_IsRunning && m_Animator != null)
        {
            m_Animator.SetBool("IsRunning", true);
            m_IsRunning = true; 
        }

        if (m_EnemyCombat != null)
        {

            if (distanceToTarget <= m_AttackRange && Vector3.Angle(direction, m_EnemyMovement.transform.forward) < m_AttackingAngle)
            {
                {
                    m_EnemyCombat.StartAttacking(true);
                    m_NavMeshAgent.isStopped = true;
                    m_IsRunning = false;
                    m_Animator.SetBool("IsRunning", m_IsRunning);
                }
            }
            else
            {
                m_EnemyCombat.StartAttacking(false);
                m_NavMeshAgent.isStopped = false;
                m_IsRunning = true;
                m_Animator.SetBool("IsRunning", m_IsRunning);
            }
        }
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
