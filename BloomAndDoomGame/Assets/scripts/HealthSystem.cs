using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealth;
    private float m_Health;
    [SerializeField]
    public GameObject dmgTextPrefab;

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

        // spawn popup text
        GameObject popup = Instantiate(dmgTextPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Setup(damage, false); // false to have rounded number, true otherwise

        Debug.Log("Health: " + m_Health);
        if (m_Health <= 0)
        {
            //TODO DIE !!!
            Debug.Log("Character died");
        }
    }
}
