using UnityEngine;

public class DefenseCollectable : Item
{
          private int newValue = 30;

          void Start()
          {
                    _price = 15;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetCurrency(playerStats.GetCurrency() - _price);
                              playerStats.SetDefense(playerStats.GetDefense() + newValue);
                              Destroy(gameObject);
                    }
          }
}