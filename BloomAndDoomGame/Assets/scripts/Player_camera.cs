using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [Header("Références")]
    public Transform target; // Votre personnage
    public Transform pivotPoint; // Point de pivot (optionnel)
    
    [Header("Paramètres de distance")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    
    [Header("Paramètres de rotation")]
    public float mouseSensitivity = 2f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    
    [Header("Paramètres de lissage")]
    public float rotationSmoothTime = 0.1f;
    public float positionSmoothTime = 0.1f;
    
    [Header("Options")]
    public bool invertYAxis = false;
    public bool lockCursor = true;
    
    // Variables privées
    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 currentVelocity;
    private Vector2 rotationVelocity;
    private Vector2 lookInput;
    private float scrollInput;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        // Initialiser currentX avec la rotation Y du personnage
        Vector3 angles = transform.eulerAngles;
        currentX = target != null ? target.eulerAngles.y : angles.y;
        currentY = angles.x;
    }

    // Méthodes pour l'Input System
    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
    
    void OnScroll(InputValue value)
    {
        scrollInput = value.Get<Vector2>().y;
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        HandleMouseInput();
        UpdateCameraPosition();
        HandleScrollWheel();
    }
    
    void HandleMouseInput()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        
        if (invertYAxis)
            mouseY = -mouseY;
        
        // Ajuster currentX en fonction de la rotation du personnage + input souris
        float targetX = target.eulerAngles.y + mouseX;
        float targetY = currentY - mouseY;
        
        targetY = Mathf.Clamp(targetY, minVerticalAngle, maxVerticalAngle);
        
        currentX = Mathf.SmoothDampAngle(currentX, targetX, ref rotationVelocity.x, rotationSmoothTime);
        currentY = Mathf.SmoothDampAngle(currentY, targetY, ref rotationVelocity.y, rotationSmoothTime);
    }
    
    void UpdateCameraPosition()
    {
        Vector3 pivotPosition = pivotPoint != null ? pivotPoint.position : target.position;
        
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = rotation * Vector3.back;
        Vector3 desiredPosition = pivotPosition + direction * distance;
        
        desiredPosition = CheckForCollisions(pivotPosition, desiredPosition);
        
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);
        transform.LookAt(pivotPosition);
    }
    
    void HandleScrollWheel()
    {
        if (Mathf.Abs(scrollInput) > 0.1f)
        {
            distance -= scrollInput * 2f;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }
    
    Vector3 CheckForCollisions(Vector3 pivotPosition, Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - pivotPosition;
        float targetDistance = direction.magnitude;
        
        RaycastHit hit;
        if (Physics.Raycast(pivotPosition, direction.normalized, out hit, targetDistance))
        {
            return hit.point - direction.normalized * 0.2f;
        }
        
        return desiredPosition;
    }
    
    void OnEscape(InputValue value)
    {
        if (value.isPressed)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}