using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Handles player movement/jump and drives animator ground/fall states.
*/
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera camera;
    [SerializeField] private Transform yawTarget;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool shouldFaceMoveDirection = true;

    [Header("Ground Check")]
    private LayerMask groundLayerMask = ~0;
    private float groundProbeExtra = 0.08f;
    private float groundRadiusPadding = 0.02f;

    [Header("Jump Leniency")]
    private float coyoteTime = 0.10f;      // after leaving ground
    private float jumpBufferTime = 0.10f;  // before landing

    [Header("Interactor")] // For collectibles
    public float InteractRange;

    private CharacterController controller;
    private AnimationStateController animationState;

    private Vector2 moveInput;
    private Vector3 velocity;

    // leniency timers
    private float lastGroundedAt = -999f;
    private float lastJumpPressedAt = -999f;

    public void Start()
    {
        camera = Camera.main;

        if (!camera.TryGetComponent(out CinemachineBrain _))
        {
            camera.gameObject.AddComponent<CinemachineBrain>();
        }

        controller = GetComponent<CharacterController>();
        animationState = GetComponent<AnimationStateController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        animationState?.OnRun(moveInput);
    }

    private void OnJump(InputValue value)
    {
        lastJumpPressedAt = Time.time;
    }

    // private void OnInteract(InputValue value)
    // {
    //     Debug.Log("Is trying to interact with IInterable object");
    //     Ray r = new Ray(camera.transform.position, camera.transform.forward);
    //     if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
    //     {
    //         if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
    //         {
    //             interactObj.Interact();
    //         }
    //     }
    // }

    private void Update()
    {
        bool grounded = GroundCheck();
        UpdateGroundedState(grounded);
        HandleJump(grounded);
        MovePlayer(grounded);
        HandleRotation();
        UpdateAnimationStates(grounded);
    }

    private void UpdateGroundedState(bool grounded)
    {
        if (grounded)
            lastGroundedAt = Time.time;
    }

    private void HandleJump(bool grounded)
    {
        bool canCoyote = (Time.time - lastGroundedAt) <= coyoteTime;
        bool jumpBuffered = (Time.time - lastJumpPressedAt) <= jumpBufferTime;

        if (jumpBuffered && canCoyote)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastJumpPressedAt = -999f;
            lastGroundedAt = -999f;
        }
    }

    private void MovePlayer(bool grounded)
    {
        if(camera == null) camera = Camera.main;
        Vector3 camF = camera.transform.forward; camF.y = 0f; camF.Normalize();
        Vector3 camR = camera.transform.right; camR.y = 0f; camR.Normalize();

        Vector3 moveDir = (camF * moveInput.y + camR * moveInput.x);
        Vector3 horizontal = moveDir * speed;

        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = horizontal + new Vector3(0f, velocity.y, 0f);
        controller.Move(finalMove * Time.deltaTime);
    }

    private void UpdateAnimationStates(bool grounded)
    {
        bool isFalling = !grounded && velocity.y < 0f;
        animationState.UpdateFallState(isFalling);
    }

    private void HandleRotation()
    {
        if (!shouldFaceMoveDirection || yawTarget == null) return;

        Vector3 lookDirection = yawTarget.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private bool GroundCheck()
    {
        Vector3 center = controller.bounds.center;
        float feetY = controller.bounds.min.y;
        float radius = Mathf.Max(0.01f, controller.radius - groundRadiusPadding);

        // probe origin slightly above the sole to avoid starting inside colliders
        Vector3 probeCenter = new Vector3(center.x, feetY + radius + 0.01f, center.z);

        // Cast a short distance downward
        float maxDist = radius + groundProbeExtra;

        // Prefer SphereCast (robust on slopes/edges)
        bool hit = Physics.SphereCast(
            origin: probeCenter,
            radius: radius,
            direction: Vector3.down,
            hitInfo: out _,
            maxDistance: maxDist,
            layerMask: groundLayerMask,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore
        );

        return hit;
    }
}
