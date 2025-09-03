using UnityEngine;

public class DefenseCollectable : Item
{
          void Start()
          {
                    _price = 15f;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetDefense(playerStats.GetDefense() + 30);
                              Destroy(gameObject);
                    }
          }
}