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
        //animator = GetComponent<Animator>();
        isFallingId = Animator.StringToHash("IsFalling");
        velocityXId = Animator.StringToHash("VelocityX");
        velocityYId = Animator.StringToHash("VelocityY");
        attackSpeedId = Animator.StringToHash("AttackSpeed");
        shootingId = Animator.StringToHash("Shoot");
        isShootingId = Animator.StringToHash("IsAttacking");
        punchId = Animator.StringToHash("Punch");
        meleeAttackId = Animator.StringToHash("MeleeAttack");
        spell1Id = Animator.StringToHash("Spell1");
        spell2Id = Animator.StringToHash("Spell2");
        spell3Id = Animator.StringToHash("Spell3");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();        
    }

    private bool IsAnimatorValid()
    {
        return animator != null && animator.gameObject != null && animator.isActiveAndEnabled;
    }

    public void OnRun(Vector2 inputs)
    {
        if (!IsAnimatorValid()) return;

        animator.SetFloat(velocityXId, inputs.x);
        animator.SetFloat(velocityYId, inputs.y);
    }

    public void UpdateFallState(bool grounded)
    {
        if (!IsAnimatorValid()) return;

        animator.SetBool(isFallingId, grounded);
    }

    public void SetAttackSpeed(float amount)
    {
        animator.SetFloat(attackSpeedId, amount);
    }

    public void OnStartShoot()
    {
        if (!IsAnimatorValid()) return;

        animator.SetTrigger(shootingId);
        animator.SetBool(isShootingId, true);
    }

    public void OnStopShoot()
    {
        if (!IsAnimatorValid()) return;

        animator.SetBool(isShootingId, false);
    }

    public void OnShootEvent()
    {
        OnShootCallback?.Invoke();
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

    public void OnSpell3()
    {
        animator.SetTrigger(spell3Id);
    }

    public bool HasValidAnimator() {
        return IsAnimatorValid();
    }

    private void OnDestroy() {
        OnShootCallback = null;
        animator = null;
    }
}
