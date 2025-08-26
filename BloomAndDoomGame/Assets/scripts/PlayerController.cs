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

    public Animator animator;

    [SerializeField] GameObject arrow_spawn;

    [SerializeField] GameObject arrow;
    private AnimationStateController animationState; // Add this
    private Vector2 moveInput;
    private Vector3 velocity;

    [SerializeField] private float arrow_velocity = 30f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>(); // Add this line
        animator = GetComponent<Animator>();

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
        Debug.Log($"Move Input: {moveInput}");

        // Update animation based on movement
        if (animationState != null)
        {
            animationState.OnRun(moveInput);
        }
    }

    void OnJump(InputValue value)
    {
        Debug.Log($"Jumping {value.isPressed} - Is Grounded: {controller.isGrounded}");
        if (value.isPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Update jump animation
            if (animationState != null)
            {
                animationState.UpdateFallState(true);
            }
        }
    }

    void OnShoot(InputValue value)
    {
        animator.SetTrigger("Shoot");


        Vector3 pos = arrow_spawn.transform.position + arrow_spawn.transform.forward * 2f;

        print($"position: {pos}");

        GameObject arr = Instantiate(arrow, pos, arrow_spawn.transform.rotation);
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrow_velocity, ForceMode.Impulse);
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

        // En mode visée : mouvement relatif à la caméra (comme en mode normal)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
        moveDirection = forward * moveInput.y + right * moveInput.x;

        // Debug pour voir si on bouge
        if (moveDirection.magnitude > 0.1f)
        {
            Debug.Log($"Moving: {moveDirection} | Speed: {speed} | Input: {moveInput}");
        }

        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    void HandleRotation()
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

            // Update landing animation
            if (animationState != null)
            {
                animationState.UpdateFallState(false);
            }
        }
    }
}