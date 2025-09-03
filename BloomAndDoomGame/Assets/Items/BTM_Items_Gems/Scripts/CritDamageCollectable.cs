using UnityEngine;

public class CritDamageCollectable : Item
{
          private int newValue = 3;

          void Start()
          {
                    _price = 30;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetCurrency(playerStats.GetCurrency() - _price);
                              playerStats.SetCritDamage(playerStats.GetCritDamage() + newValue);
                              Destroy(gameObject);
                    }
          }
}