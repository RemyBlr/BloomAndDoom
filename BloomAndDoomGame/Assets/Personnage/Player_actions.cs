using UnityEngine;
using UnityEngine.InputSystem;

public class Player_actions : MonoBehaviour
{
          [Header("Paramètres")]
          [SerializeField] private float damage = 1f;

          void Start()
          {

          }

          private void OnAttack(InputValue value)
          {
                    Debug.Log("Fired!");
                    
          }

          void Update()
          {

          }
}