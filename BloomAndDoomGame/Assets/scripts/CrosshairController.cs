using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float crossHairOffsetMultiplier = 0.01f;
    [SerializeField] private LayerMask raycastMask = ~0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f - 30f, Screen.height / 2f - 30f, 0);
        Ray ray = aimCamera.ScreenPointToRay(screenCenter);

        Vector3 targetPos;
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastMask))
        {
            targetPos = hit.point + hit.normal * crossHairOffsetMultiplier;
            crosshairUI.rotation = Quaternion.LookRotation(hit.normal);
            Debug.DrawLine(hit.point, hit.point + hit.normal * 2f, Color.green);
        }
        else
        {
            targetPos = ray.GetPoint(maxDistance);
            crosshairUI.forward = aimCamera.transform.forward;
        }

        crosshairUI.position = targetPos;
    }
}
