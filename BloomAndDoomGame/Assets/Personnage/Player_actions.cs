using UnityEngine;
using UnityEngine.InputSystem;

public class Player_actions : MonoBehaviour
{
          [Header("Paramètres")]
          public Animator animator;
          [SerializeField] private float damage = 1f;

          private void Start()
          {
              animator = GetComponent<Animator>();
          }

          private void OnAttack(InputValue value)
          {
              animator.SetTrigger("Punch");
              Debug.Log("Fired!");
                    
          }
}