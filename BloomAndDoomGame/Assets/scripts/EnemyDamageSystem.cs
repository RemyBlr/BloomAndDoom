using UnityEngine;

public class EnemyDamageSystem : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats Stats;
    public float currentHeal;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;

    private Animator animator;
    public bool IsDead => currentHeal <= 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHeal = Stats.maxHealth;
    }

    public void TakeDamage(float value)
    {
        if (IsDead) return;

        float damage = CalculateDamage(value);
        currentHeal -= damage;

        SpawnPopup(damage);

        if (IsDead) {
            animator.SetTrigger("IsDead");
            GetComponent<Collider>().enabled = false;
        }
    }

    private float CalculateDamage(float rawDamage)
    {
        return Mathf.Max(1f, rawDamage - Stats.defense); // minimum 1 dégât
    }

    private void SpawnPopup(float damage) {
        if (damagePopupPrefab == null) return;

        Vector3 popupSpawnPoint = transform.position + Vector3.up * 2f;

        GameObject popupObj = Instantiate(damagePopupPrefab, popupSpawnPoint, Quaternion.identity);
        DamagePopup popup = popupObj.GetComponent<DamagePopup>();

        if (popup != null)
            popup.Setup(damage);
    }
}
