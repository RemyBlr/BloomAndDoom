using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    // text moves up at that speed
    private float moveSpd = 1f;
    private float lifetime = 1f;
    private TextMeshPro textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, bool showDecimals = false)
    {
        if (showDecimals)
            // keep one decimal
            textMesh.text = damageAmount.ToString("F1");
        else
            textMesh.text = Mathf.RoundToInt(damageAmount).ToString();
    }

    void Update()
    {
        // moves text upwards
        transform.position += new Vector3(0, moveSpd * Time.deltaTime, 0);

        // change alpha to make it disapear
        Color c = textMesh.color;
        c.a -= Time.deltaTime / lifetime;
        textMesh.color = c;

        // remove text completly
        if (c.a <= 0)
            Destroy(gameObject);
    }
}
