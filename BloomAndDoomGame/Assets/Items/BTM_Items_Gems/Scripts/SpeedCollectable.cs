using UnityEngine;

public class SpeedCollectable : Item
{
          private float newValue = 1f;

          void Start()
          {
                    _price = 30f;
          }

          public override void Interact(Interactor interactor)
          {
                    if (interactor != null)
                    {
                              CharacterStats playerStats = interactor.GetComponent<CharacterStats>();
                              playerStats.SetSpeed(playerStats.GetSpeed() + newValue);
                              Destroy(gameObject);
                    }
          }
}