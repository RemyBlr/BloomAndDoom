using UnityEngine;

public class ArrowSpawn : MonoBehaviour 
{
    public Camera playerCamera;
    
    void Update()
    {
        // Ray depuis le centre de l'écran
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        
        // Point cible (soit un objet touché, soit un point lointain)
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 1000f;
        }
        
        // Orienter l'arrowSpawn vers ce point
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
