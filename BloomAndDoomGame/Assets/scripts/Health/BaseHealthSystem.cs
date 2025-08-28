using UnityEngine;

// Interface for every entity that can take damage
public interface I_Damageable {
    void TakeDamage(float damage);
    float GetCurrentHealth();
    float GetMaxHealth();
    bool IsDead();
}

public abstract class BaseHealthSystem : MonoBehaviour, I_Damageable {

    [Header("Settings")]
    [SerializeField] protected bool showDamagePopup = false;
    [SerializeField] protected GameObject dmgTextPrefab;

    protected int popupCount = 0;

    // Abstract methods
    public abstract void TakeDamage(float damage);
    public abstract float GetCurrentHealth();
    public abstract float GetMaxHealth();
    public abstract bool IsDead();

    // Virtual methods
    protected virtual void OnDeath() {
        Debug.Log($"Mort de {gameObject.name}");
    }
    
    protected virtual void OnDamageTaken(float damage) {
        Debug.Log($"{gameObject.name} prend {damage} dégâts");
    }

    // Damage popup
    protected void CreateDamagePopup(float damage) {
        if (!showDamagePopup || dmgTextPrefab == null) {
            Debug.Log("dmgTextPrefab pas assigné sur " + gameObject.name);
            return;
        }
        
        Vector3 popupPos = transform.position + Vector3.up * 2f;
        popupPos += new Vector3(
            Random.Range(-0.5f, 0.5f),  // random horizontal movement
            popupCount * 0.3f,          // vertical movement
            Random.Range(-0.5f, 0.5f)   // random depth movement
        );
        
        // spawn popup text
        GameObject popup = Instantiate(dmgTextPrefab, popupPos, Quaternion.identity);
        DamagePopup popupScript = popup.GetComponent<DamagePopup>();
        
        if (popupScript != null)
            popupScript.Setup(damage, false); // false to have rounded number, true otherwise
        else {
            Debug.Log("Pas de composant DamagePopup sur le prefab dmgTextPrefab");
            Destroy(popup);
        }
        
        popupCount++;
        // reset popup counter
        if (popupCount > 5) popupCount = 0;
    }
}
