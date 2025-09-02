using System;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            print($"TestCollider");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            print($"TestCollider");
        }
    }
}
