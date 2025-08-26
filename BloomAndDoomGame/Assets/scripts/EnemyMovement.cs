using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private string m_PlayerTag = "Player";

    private EnemyState m_CurrentState;
    private GameObject m_PlayerObject;
    private Vector3 m_PlayerPosition;
    [SerializeField]
    private float m_AttackRange = 0.5f;
    [SerializeField]
    private float m_RotationSpeed = 5f;
    bool m_IsMoving = false;
    Animator m_Animator;

    NavMeshAgent m_NavMeshAgent;
    EnemyCombat m_EnemyCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_CurrentState = new EnemyWanderState(this, GetComponent<EnemyPerception>());
        m_Animator = GetComponent<Animator>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_EnemyCombat = GetComponent<EnemyCombat>();
        m_PlayerObject = GameObject.FindGameObjectWithTag(m_PlayerTag);
        if (m_PlayerObject == null)
        {
            Debug.LogError("Player object with tag " + m_PlayerTag + " not found.");
        }
        else
        {
            Debug.Log("Player found: " + m_PlayerObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerPosition = m_PlayerObject.transform.position;
        if (m_PlayerPosition != null)
        {

            // Check distance to player
            float distanceToPlayer = Vector3.Distance(transform.position, m_PlayerPosition);

            if (distanceToPlayer > m_AttackRange)
            {
                m_NavMeshAgent.isStopped = false;
                m_NavMeshAgent.SetDestination(m_PlayerPosition);
                m_IsMoving = true;

                //turn towards player
                Vector3 direction = (m_PlayerPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_RotationSpeed);

                if (m_EnemyCombat != null)
                {
                    m_EnemyCombat.StartAttacking(false);
                }
            }
            else
            {
                m_NavMeshAgent.isStopped = true;
                m_IsMoving = false;



                if (m_EnemyCombat != null)
                {
                    m_EnemyCombat.StartAttacking(true);
                }
            }
            if (m_Animator != null)
            {
                m_Animator.SetBool("isRunning", m_IsMoving);
            }
        }
    }
    public string GetTargetTag()
    {
        return m_PlayerTag;
    }

    public void ChangeState(EnemyState newState)
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.ExitState();
        }
        m_CurrentState = newState;
        if (m_CurrentState != null)
        {
            m_CurrentState.EnterState();
        }
    }
}
