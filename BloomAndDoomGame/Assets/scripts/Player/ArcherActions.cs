using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Gère les actions spécifiques (Input actions) de la classe "Archer"
*/
public class ArcherActions : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject arrow_spawn;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowVelocity = 75f;
    private PlayerControls controls;
    [Header("Cooldown")]

    [SerializeField] private float shootCD = 0.3f; // Cooldown duration in seconds
    private bool shootReady = true;

    private AnimationStateController animationState;
    private Animator animator;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.Shoot.started += OnStartShoot;
        controls.Gameplay.Shoot.canceled += OnStopShoot;
        
        animationState = GetComponent<AnimationStateController>();
        animator = GetComponent<Animator>();
        
        animationState.OnShootCallback += FireArrow;
    }

    private void OnStartShoot(InputAction.CallbackContext obj)
    {
        animationState.OnStartShoot();
    }
    
    private void OnStopShoot(InputAction.CallbackContext obj)
    {
        animationState.OnStopShoot();
    }
    
    private void FireArrow()
    {
        Vector3 pos = arrow_spawn.transform.position + arrow_spawn.transform.forward * 2f;
        Ray direction = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        GameObject arr = Instantiate(arrow, pos,Quaternion.LookRotation(direction.direction));
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrowVelocity, ForceMode.Impulse);
    }

    private void OnPunch(InputValue value)
    {
        animator.SetTrigger("Punch");
    }
}
