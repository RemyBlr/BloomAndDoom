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

    [Header("Archer controls")]

    CharacterStats playerStats;
    private PlayerControls controls;

    [SerializeField] private GameObject fireZone;

    [Header("Cooldown")]

    [SerializeField] private float spell1CD = 10f;
    [SerializeField] private float spell2CD = 15f;
    [SerializeField] private float spell3CD = 5f; // Cooldown duration in seconds

    [SerializeField] private float spell1Duration = 5f;
    [SerializeField] private float spell2Duration = 5f;
    [SerializeField] private float spell3Duration = 5f;

    private bool spell1Ready = true, spell2Ready = true, spell3Ready = true;

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
        playerStats = GetComponent<CharacterStats>();

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
        GameObject arr = Instantiate(arrow, pos, Quaternion.LookRotation(direction.direction));
        arr.GetComponent<Rigidbody>().AddForce(arrow_spawn.transform.forward * arrowVelocity, ForceMode.Impulse);
    }

    private void OnPunch(InputValue value)
    {
        animator.SetTrigger("Punch");
    }

    private void OnSpell_1(InputValue value)
    {
        if (!spell1Ready) return;
        StartCoroutine(TriggerSpell1(spell1Duration));
    }

    private void OnSpell_2(InputValue value)
    {
        if (!spell2Ready) return;
        StartCoroutine(TriggerSpell2(spell2Duration));
    }

    private void OnSpell_3(InputValue value)
    {
        if (!spell3Ready) return;
        StartCoroutine(TriggerSpell3(spell3Duration));
    }

    //
    // Spell activation
    //
    private IEnumerator TriggerSpell1(float cooldown)
    {
        spell1Ready = false;
        playerStats.SetAttackSpeed(2f);
        yield return new WaitForSeconds(cooldown);
        playerStats.SetAttackSpeed(1f); // normal

        yield return new WaitForSeconds(spell1CD); // cooldown after use
        spell1Ready = true;
    }

    private IEnumerator TriggerSpell2(float cooldown)
    {
        spell2Ready = false;
        playerStats.SetSpeed(8f);
        yield return new WaitForSeconds(cooldown);
        playerStats.SetSpeed(5f);

        yield return new WaitForSeconds(spell2CD); // cooldown after use
        spell2Ready = true;
    }

    private IEnumerator TriggerSpell3(float cooldown)
    {
        spell3Ready = false;

        // Activate fire zone of player
        fireZone.SetActive(true);
        yield return new WaitForSeconds(cooldown);
        fireZone.SetActive(false);

        yield return new WaitForSeconds(spell3CD); // cooldown after use
        spell3Ready = true;
    }
}
