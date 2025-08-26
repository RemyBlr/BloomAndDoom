using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform target;
    public float height = 50f;
    public bool rotateWithPlayer = true;

    void LateUpdate()
    {
        if (!target) return;

        // Camera is above player
        Vector3 pos = target.position;
        pos.y += height;
        transform.position = pos;

        if (rotateWithPlayer)
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        else
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // North is up
    }
}
