using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyProjectile : MonoBehaviour
{
          void OnCollisionEnter(Collision collision)
          {
                    if (gameObject != null && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy"))
                    {
                              Destroy(gameObject);
                    }
          }
}