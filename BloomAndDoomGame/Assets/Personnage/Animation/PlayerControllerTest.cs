using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTest : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool shouldFaceMoveDirection = true;
    [SerializeField] private Transform yawTarget;
    
    [SerializeField] private GameObject arrow_spawn;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrow_velocity = 30f;
    
    private CharacterController controller;
    private AnimationStateController animationState; // Add this
    private Vector2 moveInput;
    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>(); // Add this line
        animationState.OnShootCallback += FireArrow;
        //animator = GetComponent<Animator>();

        // Si pas de caméra assignée, utiliser la caméra principale
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    // Méthodes automatiques du New Input System (comme dans Player_movement)
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Update animation based on movement
        if (animationState != null) animationState.OnRun(moveInput);
    }

    private void OnJump(InputValue value)
    {
        Debug.Log($"Jumping {value.isPressed} - Is Grounded: {controller.isGrounded}");
        if (value.isPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Update jump animation
            if (animationState != null) animationState.UpdateFallState(true);
        }
    }

    private void OnShoot(InputValue value)
    {
        animationState.OnShoot();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleGravity();
    }

    private void FireArrow()
    {
        Vector3 pos = arrow_spawn.transform.position + arrow_spawn.transform.forward * 2f;
        GameObject arr = Instantiate(arrow, pos, arrow_spawn.transform.rotation);
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrow_velocity, ForceMode.Impulse);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // En mode visée : mouvement relatif à la caméra (comme en mode normal)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
        moveDirection = forward * moveInput.y + right * moveInput.x;
        
        // Debug pour voir si on bouge
        //if (moveDirection.magnitude > 0.1f) Debug.Log($"Moving: {moveDirection} | Speed: {speed} | Input: {moveInput}");
        
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    private void HandleRotation()
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

    private Vector3 GetMoveDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * moveInput.y + right * moveInput.x;
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Réinitialiser la vélocité Y si on touche le sol
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            // Update landing animation
            if (animationState != null)
            {
                animationState.UpdateFallState(false);
            }
        }
    }
}
