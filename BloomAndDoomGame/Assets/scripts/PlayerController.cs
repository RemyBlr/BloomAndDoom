using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private bool shouldFaceMoveDirection = true; // ← Activez ceci pour TPS
    [SerializeField] private Transform yawTarget;

    private CharacterController controller;
    private AnimationStateController animationState;
    private Vector2 moveInput;
    private Vector3 velocity;
    public bool isAiming;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>();

        // Si pas de caméra assignée, utiliser la caméra principale
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Méthodes automatiques du New Input System (comme dans Player_movement)
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        animationState.OnRun(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && controller.isGrounded)
        {
            animationState.UpdateFallState(true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleGravity();
    }
    
    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (isAiming)
        {
            // En mode visée : mouvement relatif au personnage
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();
            moveDirection = forward * moveInput.y + right * moveInput.x;
        }
        else
        {
            // En mode normal : mouvement relatif à la caméra
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();
            moveDirection = forward * moveInput.y + right * moveInput.x;
        }
        
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
    
    void HandleRotation()
    {
        if (isAiming)
        {
            // En mode visée : regarder vers yawTarget
            if (yawTarget != null)
            {
                Vector3 lookDirection = yawTarget.forward;
                lookDirection.y = 0;

                if (lookDirection.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }
        }
        else if (shouldFaceMoveDirection)
        {
            // En mode normal : tourner vers la direction du mouvement
            Vector3 moveDirection = GetMoveDirection();
            if (moveDirection.sqrMagnitude > 0.001f)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
            }
        }
    }
    
    Vector3 GetMoveDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
        
        return forward * moveInput.y + right * moveInput.x;
    }
    
    void HandleGravity()
    {
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