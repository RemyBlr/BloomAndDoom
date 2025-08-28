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

    [Header("Cooldown")]

    [SerializeField] private float shootCD = 0.3f; // Cooldown duration in seconds
    private bool shootReady = true;

    private AnimationStateController animationState;
    private Animator animator;

    void Start()
    {
        animationState = GetComponent<AnimationStateController>();
        animator = GetComponent<Animator>();
    }

    void OnShoot(InputValue value)
    {
        if (shootReady == false)
            return;

        StartCoroutine(Shoot(shootCD));
    }

    IEnumerator Shoot(float cooldown)
    {
        shootReady = false;

        animator.SetTrigger("Shoot");
        GameObject arr = Instantiate(arrow, arrow_spawn.transform.position, arrow_spawn.transform.rotation);
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrowVelocity, ForceMode.Impulse);

        yield return new WaitForSeconds(cooldown); // Démarre l'attente du cooldown

        shootReady = true;
    }

    void OnPunch(InputValue value)
    {
        animator.SetTrigger("Punch");
    }
}
