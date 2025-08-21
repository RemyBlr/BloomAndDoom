using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_actions : MonoBehaviour
{
    [Header("Paramètres")]
    public Animator animator;

    [SerializeField] GameObject arrow_spawn;

    [SerializeField] Rigidbody arrow;

    [SerializeField] private float arrow_velocity = 25f;

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
        Debug.Log("Terrorizing enemies around.");

        Rigidbody arr = Instantiate(arrow, arrow_spawn.transform.position, arrow_spawn.transform.rotation);
        arr.AddForce(arrow_spawn.transform.forward * arrow_velocity, ForceMode.Impulse);
    }
}