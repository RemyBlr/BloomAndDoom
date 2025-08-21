using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    Vector3 m_PlayerPosition;
    [SerializeField]
    float m_AttackRange = 0.5f;
    bool m_IsMoving = false;
    Animator m_Animator;

    NavMeshAgent m_NavMeshAgent;
    EnemyCombat m_EnemyCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_EnemyCombat = GetComponent<EnemyCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerPosition != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

            if (distanceToPlayer > m_AttackRange)
            {
                m_NavMeshAgent.isStopped = false;
                m_NavMeshAgent.SetDestination(m_PlayerPosition);
                m_IsMoving = true;
                m_EnemyCombat.StartAttacking(false);
            }
            else
            {
                m_NavMeshAgent.isStopped = true;
                m_IsMoving = false;
                m_EnemyCombat.StartAttacking(true);
            }
            if (m_Animator != null)
            {
                m_Animator.SetBool("isRunning", m_IsMoving);
            }
        }
    }
}
