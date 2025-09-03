using UnityEngine;

public class HealthCollectable : Item
{
          void Start()
          {
                    _price = 10f;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetHealth(playerStats.GetCurrentHealth() + 50);
                              Destroy(gameObject);
                    }
          }
}