using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;

    private int isRunningId;
    private int isFallingId;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        isRunningId = Animator.StringToHash("IsRunning");
        isFallingId = Animator.StringToHash("IsFalling");
    }

    public void OnRun(Vector2 inputs)
    {
        print($"inputs = {inputs}");
        bool isRunning = animator.GetBool(isRunningId);
        if (isRunning && inputs == Vector2.zero)
        {
            animator.SetBool(isRunningId, false);
        }
        else if (!isRunning && inputs != Vector2.zero)
        {
            animator.SetBool(isRunningId, true);
        }
    }

    public void UpdateFallState(bool state)
    {
        animator.SetBool(isFallingId, state);
    }
}
