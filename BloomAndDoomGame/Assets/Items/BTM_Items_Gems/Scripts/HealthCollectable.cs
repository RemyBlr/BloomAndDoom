using UnityEngine;

public class HealthCollectable : Collectable
{
          [SerializeField] private CharacterStats playerStats;

          private void OnTriggerEnter(Collider other)
          {
                    Debug.Log("Healing 50 HP !");
                    if (other.gameObject.CompareTag(collector))
                    {
                              playerStats.SetHealth(50);
                              Destroy(gameObject);
                    }
          }
}