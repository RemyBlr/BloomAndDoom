using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    
    [Header("Rotation")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    
    [Header("Caméra")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    
    [Header("Options")]
    [SerializeField] private bool invertYAxis = false;
    [SerializeField] private bool lockCursor = true;

    // Variables privées
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    
    // Angles de rotation
    private float yaw = 0f;   // Rotation horizontale (Y)
    private float pitch = 0f; // Rotation verticale (X)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Verrouiller le curseur
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        // Si pas de caméra assignée, utiliser la caméra principale
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        // Initialiser les angles avec la rotation actuelle
        yaw = transform.eulerAngles.y;
    }

    // Input System callbacks
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleGravity();
        UpdateCamera();
    }
    
    void HandleMouseLook()
    {
        // Récupérer l'input de la souris
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        
        if (invertYAxis)
            mouseY = -mouseY;
        
        // Ajuster les angles
        yaw += mouseX;
        pitch -= mouseY;
        
        // Limiter l'angle vertical
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);
        
        // Appliquer la rotation horizontale au personnage
        float smoothedYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, yaw, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
    }
    
    void HandleMovement()
    {
        // Créer le vecteur de mouvement basé sur l'input et la rotation du personnage
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        
        if (inputDirection.magnitude >= 0.1f)
        {
            // Le mouvement se fait dans la direction où regarde le personnage
            Vector3 moveDirection = transform.TransformDirection(inputDirection);
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }
    
    void HandleGravity()
    {
        // Appliquer la gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Réinitialiser la vélocité Y si on touche le sol
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    void UpdateCamera()
    {
        if (cameraTransform == null) return;
        
        // Position de base de la caméra (au-dessus du personnage)
        Vector3 targetPosition = transform.position + Vector3.up * cameraHeight;
        
        // Calculer la rotation de la caméra (combinaison yaw + pitch)
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0);
        
        // Calculer la position de la caméra derrière le personnage
        Vector3 cameraOffset = cameraRotation * Vector3.back * cameraDistance;
        Vector3 desiredCameraPosition = targetPosition + cameraOffset;
        
        // Vérifier les collisions
        desiredCameraPosition = CheckCameraCollisions(targetPosition, desiredCameraPosition);
        
        // Appliquer la position et rotation à la caméra
        cameraTransform.position = desiredCameraPosition;
        cameraTransform.LookAt(targetPosition);
    }
    
    Vector3 CheckCameraCollisions(Vector3 targetPosition, Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - targetPosition;
        float distance = direction.magnitude;
        
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, direction.normalized, out hit, distance))
        {
            return hit.point - direction.normalized * 0.2f;
        }
        
        return desiredPosition;
    }
}