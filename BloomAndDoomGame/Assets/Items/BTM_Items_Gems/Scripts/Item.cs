using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
          protected int _price = 0;
          public virtual void Interact(Interactor interactor) { }
          public float GetPrice() { return _price; }
}