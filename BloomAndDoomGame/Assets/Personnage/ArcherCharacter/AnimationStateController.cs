using System;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolder;
    private Animator animator;

    //private int isRunningId;
    private int isFallingId;
    private int velocityXId;
    private int velocityYId;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //isRunningId = Animator.StringToHash("IsRunning");
        isFallingId = Animator.StringToHash("IsFalling");
        velocityXId = Animator.StringToHash("VelocityX");
        velocityYId = Animator.StringToHash("VelocityY");
    }

    public void OnRun(Vector2 inputs)
    {
        //print($"inputs = {inputs}");
        animator.SetFloat(velocityXId, inputs.x);
        animator.SetFloat(velocityYId, inputs.y);
    }

    public void UpdateFallState(bool state)
    {
        animator.SetBool(isFallingId, state);
    }

    public void EnableCamera()
    {
        cameraHolder.SetActive(true);
    }
}
