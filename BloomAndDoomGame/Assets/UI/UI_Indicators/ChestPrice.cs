using UnityEngine;
using TMPro;

public class ChestPrice : MonoBehaviour
{
    [Header("Chest Settings")]
    public int price = 100;

    [Header("Price UI")]
    public GameObject chestPricePrefab;
    private TextMeshPro priceText;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;   

        GameObject ui = Instantiate(chestPricePrefab, transform);

        priceText = ui.GetComponentInChildren<TextMeshPro>();
        if (priceText != null)
            priceText.text = price.ToString() + "$";
    }

    void LateUpdate()
    {
        if(mainCamera != null)
            transform.LookAt(transform.position + mainCamera.transform.forward);
    }

    public void UpdatePrice(int newPrice)
    {
        price = newPrice;
        if (priceText != null)
            priceText.text = newPrice + "$";
    }
}
