using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    // text moves up at that speed
    private float moveSpd = 1f;
    private float lifetime = 1f;
    private TextMeshPro textMesh;
    private Camera playerCamera;
    private float timer = 0f;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();

        playerCamera = Camera.main;

        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>(); // next found camera if main not found
        
        if (textMesh == null)
            Debug.LogError("composant TextMeshPro pas trouvé pour DamagePopup!");
    }

    public void Setup(float damageAmount, bool showDecimals = false)
    {
        if (textMesh != null) {
            if (showDecimals)
                // keep one decimal
                textMesh.text = damageAmount.ToString("F1");
            else
                textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
        }
    }

    void Update()
    {
        if (textMesh == null) return;

        timer += Time.deltaTime;

        // moves text upwards
        transform.position += new Vector3(0, moveSpd * Time.deltaTime, 0);

        // face text to camera
        if (playerCamera != null)
            transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward, playerCamera.transform.rotation * Vector3.up);

        // change alpha to make it disapear
        Color c = textMesh.color;
        c.a = 1f - (timer / lifetime);
        textMesh.color = c;

        // remove text completly when timer exceeds lifetime
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
