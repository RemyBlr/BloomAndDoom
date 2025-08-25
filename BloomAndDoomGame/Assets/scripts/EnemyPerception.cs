using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField]
    private string m_TargetTag = "Player";

    [SerializeField]
    private float m_DetectionRange = 10f;

    [SerializeField]
    private float m_TargetHuggingDistance = 0.5f;
    private float m_TargetHuggingDistanceSqr;
    private float m_DetectionRangeSqr;

    [SerializeField]
    private float m_FieldOfView = 120f; // this FOV is easier to understand

    [SerializeField]
    private Transform m_Eyes;

    [Header("Obstacle Detection")]
    [Tooltip("Layers considered as obstacles for line of sight checks")]
    [SerializeField]
    private LayerMask m_ObstacleMask = LayerMask.GetMask("Environment", "Terrain"); // This is used to cancel out obstacles

    [Tooltip("Radius for obstacle detection checks")]
    [SerializeField, Min(0f)]
    private float m_ObstacleCheckRadius = 0.1f; // Radius for sphere cast to check obstacles

    private float m_HalfFovCosine; //This is used for the calculations

    private GameObject m_TargetObject;


    void Awake()
    {
        if (m_Eyes == null) m_Eyes = transform;

        m_TargetObject = GameObject.FindGameObjectWithTag(m_TargetTag);
        m_HalfFovCosine = Mathf.Cos(m_FieldOfView / 2f * Mathf.Deg2Rad); // We only calculate once the half FOV cosine to optimize performance
        m_DetectionRangeSqr = m_DetectionRange * m_DetectionRange; // Same for the squared detection range
        m_TargetHuggingDistanceSqr = m_TargetHuggingDistance * m_TargetHuggingDistance; // Same for the squared target hugging distance
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //Note: this weird way of checking avoid lots of recalculations
    public bool IsInFieldOfView()
    {
        if (m_TargetObject == null || m_Eyes == null)
        {
            Debug.LogError("Target object or eyes transform is not assigned.");
            return false;
        }

        // Get the closest point on the target's collider
        Vector3 targetPoint = m_TargetObject.transform.position;
        if (m_TargetObject.TryGetComponent<Collider>(out Collider targetCollider))
        {
            targetPoint = targetCollider.ClosestPoint(m_Eyes.position);
        }
        Vector3 vectorToTarget = (targetPoint - m_Eyes.position);

        //check the distance
        float distance = Vector3.SqrMagnitude(vectorToTarget);
        if (distance > m_DetectionRangeSqr) return false;
        if (distance < m_TargetHuggingDistanceSqr) return true; // if the target is extremely close, consider it visible

        //check the angle
        Vector3 directionToTarget = vectorToTarget.normalized; // we reuse this in sphere cast
        float dotProduct = Vector3.Dot(m_Eyes.forward, directionToTarget);
        if (dotProduct < m_HalfFovCosine) return false;


        //check to see if there are any obstacles in the way
        if (Physics.SphereCast(m_Eyes.position, m_ObstacleCheckRadius, directionToTarget, out RaycastHit hitInfo, Mathf.Sqrt(distance), m_ObstacleMask, QueryTriggerInteraction.Ignore)) return false;

        return true;
    }

    //This function is called when the script is loaded or a value is changed in the inspector
    void OnValidate()
    {
        m_ObstacleCheckRadius = Mathf.Max(0f, m_ObstacleCheckRadius);
        m_FieldOfView = Mathf.Clamp(m_FieldOfView, 0f, 180f);
        m_DetectionRange = Mathf.Max(0f, m_DetectionRange);
        m_HalfFovCosine = Mathf.Cos(m_FieldOfView / 2f * Mathf.Deg2Rad);
        m_DetectionRangeSqr = m_DetectionRange * m_DetectionRange;
        m_TargetHuggingDistance = Mathf.Max(0f, m_TargetHuggingDistance);
        m_TargetHuggingDistanceSqr = m_TargetHuggingDistance * m_TargetHuggingDistance;
        if (m_Eyes == null) m_Eyes = transform;
    }



}
