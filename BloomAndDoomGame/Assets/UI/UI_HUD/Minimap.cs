using UnityEngine;

public class Minimap : MonoBehaviour
{
    [HideInInspector]public Transform target;

    [Header("Camera offset")]
    public Vector3 offset = new Vector3(0, 30f, -20f);

    [Header("Camera angle")]
    public Vector3 rotation = new Vector3(45f, 0f, 0f);
    //public Vector3 rotation = new Vector3(45f, target.eulerAngles.y, 0f); // minimap turns with player

    private void Start()
    {
        if (target == null)
            FindPlayerTarget();
    }

    private void LateUpdate()
    {
        if (target == null) {
            FindPlayerTarget();
            return;
        }

        // Camera follows player
        transform.position = target.position + offset;

        // Camera angle
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void FindPlayerTarget()
    {
        // Check for player tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            return;
        }

        // Check in GameManager
        if (GameManager.Instance != null && GameManager.Instance.playerInstance != null)
        {
            target = GameManager.Instance.playerInstance.transform;
            return;
        }
    }
}