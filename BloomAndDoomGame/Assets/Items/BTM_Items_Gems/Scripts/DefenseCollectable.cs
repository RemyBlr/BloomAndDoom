using UnityEngine;

public class DefenseCollectable : Collectable
{
          private void OnTriggerEnter(Collider other)
          {
                    if (other.gameObject.CompareTag(collector))
                    {
                              playerStats.SetDefense(playerStats.GetDefense() + 30);
                              Destroy(gameObject);
                    }
          }
}