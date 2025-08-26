using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_actions : MonoBehaviour
{
    [Header("Paramètres")]
    public Animator animator;

    [SerializeField] GameObject arrow_spawn;

    [SerializeField] GameObject arrow;

    [SerializeField] private float arrow_velocity = 30f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnPunch(InputValue value)
    {
        animator.SetTrigger("Punch");
        Debug.Log("You punch his ego !");
    }

    private void OnShoot(InputValue value)
    {
        animator.SetTrigger("Shoot");

        GameObject arr = Instantiate(arrow, arrow_spawn.transform.position, arrow_spawn.transform.rotation);
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrow_velocity, ForceMode.Impulse);
    }

    private void OnAttack(InputValue value)
    {
        animator.SetTrigger("Aim");
        Debug.Log("Fired!");
    }
}