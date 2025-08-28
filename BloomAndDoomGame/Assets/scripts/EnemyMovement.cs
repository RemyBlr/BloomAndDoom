using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent m_NavMeshAgent { get; private set; }
    public Animator m_Animator { get; private set; }
    public EnemyCombat m_EnemyCombat { get; private set; }
    public EnemyPerception m_EnemyPerception { get; private set; }
    public GameObject m_PlayerObject { get; private set; }

    [SerializeField]
    private float m_WanderingSpeed = 2.0f;

    [SerializeField]
    private float m_ChasingSpeed = 4.0f;
    private EnemyState m_CurrentState;
    [SerializeField] private string m_PlayerTag = "Player";


    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_EnemyCombat = GetComponent<EnemyCombat>();
        m_CurrentState = new EnemyWanderState(this, GetComponent<EnemyPerception>());
        m_PlayerObject = GameObject.FindGameObjectWithTag(m_PlayerTag);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        m_CurrentState.UpdateState();
    }

    public void ChangeState(EnemyState newState)
    {
        Debug.Log("State changed from " + m_CurrentState.GetType().Name + " to " + newState.GetType().Name);
        m_CurrentState = newState;
    }

    public float GetWanderingSpeed()
    {
        return m_WanderingSpeed;
    }

    public float GetChasingSpeed()
    {
        return m_ChasingSpeed;
    }
}
