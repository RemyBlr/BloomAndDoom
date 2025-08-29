using UnityEngine;

public class Collectable : MonoBehaviour
{
    private string collector = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(collector))
        {
            Destroy(gameObject);
        }
    }
}