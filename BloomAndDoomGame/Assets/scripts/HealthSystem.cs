using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealth;
    private float m_Health;
    [SerializeField]
    public GameObject dmgTextPrefab;

    private int popupCount = 0;

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

        if(dmgTextPrefab != null) {
            Vector3 popupPos = transform.position + Vector3.up * 2f;
            popupPos += new Vector3(
                Random.Range(-0.5f, 0.5f), // random horizontal movement
                popupCount * 0.3f,         // vertical movement
                Random.Range(-0.5f, 0.5f)  // random facing movement
            );

            // spawn popup text
            GameObject popup = Instantiate(dmgTextPrefab, popupPos, Quaternion.identity);
            DamagePopup popupScript = popup.GetComponent<DamagePopup>();

            if(popupScript != null)
                popupScript.Setup(damage, false); // false to have rounded number, true otherwise
            else {
                Debug.LogError("Le prefab dmgTextPrefab n'a pas le composant DamagePopup!");
                Destroy(popup);
            }

            popupCount++;
            // reset popup counter
            if (popupCount > 5) popupCount = 0;
        }
        else
            Debug.LogWarning("dmgTextPrefab pas assigné sur " + gameObject.name);

        Debug.Log("Health: " + m_Health);
        if (m_Health <= 0)
        {
            //TODO DIE !!!
            Debug.Log("Character died");
        }
    }
}
