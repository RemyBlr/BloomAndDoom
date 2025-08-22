using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealth;
    private float m_Health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       m_Health = m_MaxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;
        Debug.Log("Health: " + m_Health);
        if (m_Health <= 0)
        {
            //TODO DIE !!!
            Debug.Log("Character died");
        }
    }
}
