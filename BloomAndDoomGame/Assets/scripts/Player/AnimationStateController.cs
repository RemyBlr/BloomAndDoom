using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;

    private int isFallingId;
    private int velocityXId;
    private int velocityYId;
    private int attackSpeedId;
    private int shootingId;
    private int isShootingId;
    private int punchId;
    private int meleeAttackId;
    private int spell1Id;
    private int spell2Id;
    private int spell3Id;

    public Action OnShootCallback;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isFallingId = Animator.StringToHash("IsFalling");
        velocityXId = Animator.StringToHash("VelocityX");
        velocityYId = Animator.StringToHash("VelocityY");
        attackSpeedId = Animator.StringToHash("AttackSpeed");
        shootingId = Animator.StringToHash("Shoot");
        isShootingId = Animator.StringToHash("IsShooting");
        punchId = Animator.StringToHash("Punch");
        meleeAttackId = Animator.StringToHash("MeleeAttack");
        spell1Id = Animator.StringToHash("Spell1");
        spell2Id = Animator.StringToHash("Spell2");
        spell3Id = Animator.StringToHash("Spell3");
    }

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

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

    // public void SetAttackSpeed(int amount)
    // {
    //     animator.SetInteger(attackSpeedId, amount);
    // }

    public void OnStartShoot()
    {
        if (animator == null) return;
        animator.SetTrigger(shootingId);
        animator.SetBool(isShootingId, true);
    }

    public void OnStopShoot()
    {
        if (animator == null) return;
        animator.SetBool(isShootingId, false);
    }

    public void OnShootEvent()
    {
        OnShootCallback?.Invoke();
    }

    public void OnPunch()
    {
        if (animator == null) return;
        animator.SetTrigger(punchId);
    }

    public void OnMeleeAttack()
    {
        if (animator == null) return;
        animator.SetTrigger(meleeAttackId);
    }

    public void OnSpell1()
    {
        animator.SetTrigger(spell1Id);
    }

    public void OnSpell2()
    {
        animator.SetTrigger(spell2Id);
    }

    // public void OnSpell3()
    // {
    //     animator.SetTrigger(spell3Id);
    // }
}
