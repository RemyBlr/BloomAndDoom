using UnityEngine;

public class HealthCollectable : Collectable
{
          private void OnTriggerEnter(Collider other)
          {
                    if (other.gameObject.CompareTag(collector))
                    {
                              playerStats.SetHealth(playerStats.GetCurrentHealth() + 50);
                              Destroy(gameObject);
                    }
          }
}