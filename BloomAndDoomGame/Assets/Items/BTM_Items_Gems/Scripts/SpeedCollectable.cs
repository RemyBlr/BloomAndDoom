using UnityEngine;

public class SpeedCollectable : Item
{
          private int newValue = 1;

          void Start()
          {
                    _price = 50;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetCurrency(playerStats.GetCurrency() - _price);
                              playerStats.SetSpeed(playerStats.GetSpeed() + newValue);
                              Destroy(gameObject);
                    }
          }
}