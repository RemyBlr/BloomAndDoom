using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
          protected float _price = 0f;
          public virtual void Interact(Interactor interactor) { }
          public float GetPrice() { return _price; }
}