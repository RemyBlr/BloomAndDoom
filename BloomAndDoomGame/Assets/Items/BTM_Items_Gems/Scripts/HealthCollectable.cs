using UnityEngine;

public class HealthCollectable : Item
{
          private int newValue = 50;
          
          void Start()
          {
                    _price = 5;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetCurrency(playerStats.GetCurrency() - _price);
                              playerStats.SetHealth(playerStats.GetCurrentHealth() + newValue);
                              Destroy(gameObject);
                    }
          }
}