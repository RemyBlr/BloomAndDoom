using UnityEngine;

public class DealMeleeDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage = 10f;

    [SerializeField]
    private string m_TargetTag = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_TargetTag))
        {
            HealthSystem VictimHealth = other.GetComponent<HealthSystem>();
            if (VictimHealth != null)
            {
                VictimHealth.TakeDamage(m_Damage);
            }
        }
    }
}
