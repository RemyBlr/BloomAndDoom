using UnityEngine;

public class DefenseCollectable : Collectable
{
          private void OnTriggerEnter(Collider other)
          {
                    Debug.Log("Defense +30 upgrade");
                    if (other.gameObject.CompareTag(collector))
                    {
                              playerStats.SetDefense(30);
                              Destroy(gameObject);
                    }
          }
}