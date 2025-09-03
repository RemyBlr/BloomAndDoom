using UnityEngine;

public class HealthCollectable : Item
{
          private float newValue = 50f;
          
          void Start()
          {
                    _price = 5f;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetHealth(playerStats.GetCurrentHealth() + newValue);
                              Destroy(gameObject);
                    }
          }
}