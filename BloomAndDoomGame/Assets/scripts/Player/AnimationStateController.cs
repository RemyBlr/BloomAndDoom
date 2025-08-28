using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolder;

    private Animator animator;

    private int isFallingId;
    private int velocityXId;
    private int velocityYId;
    private int shootingId;
    private int isGroundedId;
    private int isShootingId;
    private int PunchId;

    public Action OnShootCallback;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        isFallingId = Animator.StringToHash("IsFalling");
        velocityXId = Animator.StringToHash("VelocityX");
        velocityYId = Animator.StringToHash("VelocityY");
        shootingId = Animator.StringToHash("Shoot");
        isShootingId = Animator.StringToHash("IsShooting");
        PunchId = Animator.StringToHash("Punch");
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // --- Called by PlayerController ---

    public void OnRun(Vector2 inputs)
    {
        if (animator == null) return;
        animator.SetFloat(velocityXId, inputs.x);
        animator.SetFloat(velocityYId, inputs.y);
    }

    public void UpdateFallState(bool grounded)
    {
        if (animator == null) return;
        animator.SetBool(isFallingId, grounded);
    }

    public void OnStartShoot()
    {
        animator.SetTrigger(shootingId);
        animator.SetBool(isShootingId, true);
    }
    
    public void OnStopShoot()
    {
        animator.SetBool(isShootingId, false);
    }

    public void OnShootEvent()
    {
        OnShootCallback?.Invoke();
    }

    public void OnPunch()
    {
        animator.SetTrigger(PunchId);
    }
}
