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
    [SerializeField] private float turnSmoothTime = 0.6f;
    
    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    
    [Header("Options")]
    [SerializeField] private bool invertYAxis = false;
    [SerializeField] private bool lockCursor = true;

    // Variables privées
    private AnimationStateController animationState;
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
        animationState = GetComponent<AnimationStateController>();
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
        animationState.OnRun(moveInput);
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationState.UpdateFallState(true);
        }
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
        HandleMovement();
        HandleGravity();
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
            animationState.UpdateFallState(false);
        }
    }
}