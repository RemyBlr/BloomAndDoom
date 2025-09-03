using UnityEngine;
using TMPro;

public class ChestPrice : MonoBehaviour
{
    [Header("Price UI")]
    public GameObject chestPricePrefab;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;   
    }

    void LateUpdate()
    {
        if(mainCamera != null)
            transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
