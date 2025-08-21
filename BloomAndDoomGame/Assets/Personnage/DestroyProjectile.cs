using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyProjectile : MonoBehaviour
{
          void OnCollisionEnter(Collision collision)
          {
                    if (collision.gameObject.tag.Equals("Ground") ||
                    collision.gameObject.tag.Equals("enemy"))
                    {
                              Debug.Log("Flèche détruite !");
                              Destroy(this);
                    }
          }
}