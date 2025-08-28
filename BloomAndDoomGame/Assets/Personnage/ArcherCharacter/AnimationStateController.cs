using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolder;

    private Animator animator;
    private CharacterController characterController;

    private int isFallingId;
    private int velocityXId;
    private int velocityYId;
    private int shootingId;

    public Action OnShootCallback;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        isFallingId = Animator.StringToHash("IsFalling");
        velocityXId = Animator.StringToHash("VelocityX");
        velocityYId = Animator.StringToHash("VelocityY");
        shootingId = Animator.StringToHash("Shoot");
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


    public void OnShoot()
    {
        animator.SetTrigger(shootingId);
    }

    public void OnShootEvent()
    {
        OnShootCallback?.Invoke();
    }
        
    public void EnableCamera()
    {
        if (animator == null) return;
        animator.SetBool(isFallingId, state);
    }
}
