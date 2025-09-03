using UnityEngine;

public class CritDamageCollectable : Item
{
          private float newValue = 3f;

          void Start()
          {
                    _price = 20f;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetCritDamage(playerStats.GetCritDamage() + newValue);
                              Destroy(gameObject);
                    }
          }
}