using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolder;

    private Animator animator;
    private CharacterController characterController;

    private int isFallingId;
    private int velocityXId;
    private int velocityYId;
    private int isGroundedId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isFallingId  = Animator.StringToHash("IsFalling");
        velocityXId  = Animator.StringToHash("VelocityX");
        velocityYId  = Animator.StringToHash("VelocityY");
        isGroundedId = Animator.StringToHash("IsGrounded");
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // --- Called by PlayerController ---

    public void OnRun(Vector2 inputs)
    {
        if (animator == null) return;
        animator.SetFloat(velocityXId, inputs.x);
        animator.SetFloat(velocityYId, inputs.y);
    }

    public void SetGrounded(bool grounded)
    {
        if (animator == null) return;
        animator.SetBool(isGroundedId, grounded);
    }

    public void SetFalling(bool state)
    {
        if (animator == null) return;
        animator.SetBool(isFallingId, state);
    }
}
