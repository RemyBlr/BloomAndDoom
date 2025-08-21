using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyProjectile : MonoBehaviour
{
          void OnCollisionEnter(Collision collision)
          {
                    if (collision.gameObject.tag.Equals("Ground"))
                    {
                              Debug.Log("Flèche détruite au contact du sol");
                              Destroy(this.gameObject);
                    }
          }
}