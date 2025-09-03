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
    [SerializeField] private float spell3CD = 5f;

    [SerializeField] private float spell1Duration = 5f;
    [SerializeField] private float spell2Duration = 5f;
    [SerializeField] private float spell3Duration = 5f; // Cooldown duration in seconds

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

        if (animationState != null)
            animationState.OnShootCallback += FireArrow;
    }

    private void OnStartShoot(InputAction.CallbackContext obj)
    {
        // Has valid animator comes from AnimatioNSTateController
        if (animationState != null && animationState.HasValidAnimator() && gameObject != null)
            animationState.OnStartShoot();
    }

    private void OnStopShoot(InputAction.CallbackContext obj)
    {
        // Has valid animator comes from AnimatioNSTateController
        if (animationState != null && animationState.HasValidAnimator() && gameObject != null)
            animationState.OnStopShoot();
    }

    private void FireArrow()
    {
        Vector3 aimDirection;
        Ray direction = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        LayerMask layer = LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(direction, out RaycastHit hit, Mathf.Infinity, 1 << layer))
        {
            aimDirection = (hit.point - arrow_spawn.transform.position).normalized;
        }
        else
        {
            aimDirection = direction.direction;
        }
        Vector3 startPosition = arrow_spawn.transform.position + arrow_spawn.transform.forward * 2f;
        GameObject arr = Instantiate(arrow, startPosition, Quaternion.LookRotation(aimDirection));
        arr.GetComponent<Rigidbody>().AddForce(aimDirection * arrowVelocity, ForceMode.Impulse);
        arr.GetComponent<ArcherProjectile>().Damage = playerStats.GetAttack();
    }

    private void OnPunch(InputValue value)
    {
        if (animator != null && animator.gameObject != null)
            animator.SetTrigger("Punch");
    }

    private void OnSpell_1(InputValue value)
    {
        if (!spell1Ready || gameObject == null) return;
        StartCoroutine(TriggerSpell1(spell1Duration));
    }

    private void OnSpell_2(InputValue value)
    {
        if (!spell2Ready || gameObject == null) return;
        StartCoroutine(TriggerSpell2(spell2Duration));
    }

    private void OnSpell_3(InputValue value)
    {
        if (!spell3Ready || gameObject == null) return;
        StartCoroutine(TriggerSpell3(spell3Duration));
    }

    //
    // Spell activation
    //
    private IEnumerator TriggerSpell1(float duration)
    {
        if (playerStats == null) yield break;

        spell1Ready = false;
        playerStats.SetAttackSpeed(2f);
        yield return new WaitForSeconds(duration);

        // Check for object still existing
        if (playerStats != null)
            playerStats.SetAttackSpeed(1f);

        yield return new WaitForSeconds(spell1CD); // cooldown after use
        spell1Ready = true;
    }

    private IEnumerator TriggerSpell2(float duration)
    {
        if (playerStats == null) yield break;

        spell2Ready = false;
        playerStats.SetSpeed(8f);
        yield return new WaitForSeconds(duration);

        // Check for object still existing
        if (playerStats != null)
            playerStats.SetSpeed(5f);

        yield return new WaitForSeconds(spell2CD); // cooldown after use
        spell2Ready = true;
    }

    private IEnumerator TriggerSpell3(float duration)
    {
        if (fireZone == null) yield break;

        spell3Ready = false;

        // Activate fire zone of player
        fireZone.SetActive(true);
        yield return new WaitForSeconds(duration);

        // Check for object still existing
        if (fireZone != null)
            fireZone.SetActive(false);

        yield return new WaitForSeconds(spell3CD); // cooldown after use
        spell3Ready = true;
    }

    // Clean when obkect is destroyed
    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Gameplay.Shoot.started -= OnStartShoot;
            controls.Gameplay.Shoot.canceled -= OnStopShoot;
            controls.Disable();
            controls = null;
        }

        if (animationState != null)
            animationState.OnShootCallback -= FireArrow;

        StopAllCoroutines();
    }

    public void SetControlsEnabled(bool enabled)
    {
        if (controls != null)
        {
            if (enabled)
                controls.Enable();
            else
                controls.Disable();
        }
    }
}