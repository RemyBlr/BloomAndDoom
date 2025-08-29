using UnityEngine;

public class DefenseCollectable : Collectable
{
          [SerializeField] private CharacterStats playerStats;

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