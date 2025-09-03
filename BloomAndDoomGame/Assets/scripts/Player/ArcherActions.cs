using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * Gère les actions spécifiques (Input actions) de la classe "Archer"
*/
public class ArcherActions : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject arrow_spawn;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowVelocity = 75f;

    [Header("Controls")]
    CharacterStats playerStats;
    private PlayerControls controls;
    private AnimationStateController animationState;

    [SerializeField] private GameObject fireZone;

    [Header("Cooldown")]
    [SerializeField] private Image UISpell1;
    [SerializeField] private Image UISpell2;
    [SerializeField] private Image UISpell3;
    [SerializeField] private float spell1CD = 15f;
    [SerializeField] private float spell2CD = 10f;
    [SerializeField] private float spell3CD = 18f; // Cooldown duration in seconds

    [SerializeField] private float spell1Duration = 6f;
    [SerializeField] private float spell2Duration = 6f;
    [SerializeField] private float spell3Duration = 10f;

    private bool spell1Ready = true, spell2Ready = true, spell3Ready = true;
    private RawImage SP1, SP2, SP3;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Enable();

        // Activate UI for the Archer class
        UISpell1 = GameObject.Find("ArcherSpell1").GetComponent<Image>();
        UISpell2 = GameObject.Find("ArcherSpell2").GetComponent<Image>();
        UISpell3 = GameObject.Find("ArcherSpell3").GetComponent<Image>();

        UISpell1.gameObject.SetActive(true);
        UISpell2.gameObject.SetActive(true);
        UISpell3.gameObject.SetActive(true);

        SP1 = UISpell1.GetComponentInChildren<RawImage>();
        SP2 = UISpell2.GetComponentInChildren<RawImage>();
        SP3 = UISpell3.GetComponentInChildren<RawImage>();

        controls.Gameplay.Attack.started += (ctx) => animationState.OnStartShoot();
        controls.Gameplay.Attack.canceled += (ctx) => animationState.OnStopShoot();

        animationState = GetComponent<AnimationStateController>();
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
    private IEnumerator TriggerSpell1(float cooldown)
    {
        if (playerStats == null) yield break;

        spell1Ready = false;
        playerStats.SetAttackSpeed(2f);
        animationState.SetAttackSpeed(2f);
        SP1.color = new Color(SP1.color.r, SP1.color.g, SP1.color.b, 0.5f); // Set opacity to 50%
        yield return new WaitForSeconds(cooldown);
        playerStats.SetAttackSpeed(1f); // back to normal
        animationState.SetAttackSpeed(1f);
        yield return new WaitForSeconds(spell1CD); // cooldown after use
        SP1.color = new Color(SP1.color.r, SP1.color.g, SP1.color.b, 1f); // Restore opacity
        spell1Ready = true;
    }

    private IEnumerator TriggerSpell2(float cooldown)
    {
        if (playerStats == null) yield break;

        spell2Ready = false;
        playerStats.SetSpeed(8f);
        SP2.color = new Color(SP2.color.r, SP2.color.g, SP2.color.b, 0.5f); // Set opacity to 50%
        
        yield return new WaitForSeconds(cooldown);

        playerStats.SetSpeed(5f);

        yield return new WaitForSeconds(spell2CD); // cooldown after use

        SP2.color = new Color(SP2.color.r, SP2.color.g, SP2.color.b, 1f); // Restore opacity
        spell2Ready = true;
    }

    private IEnumerator TriggerSpell3(float cooldown)
    {
        if (fireZone == null) yield break;

        spell3Ready = false;
        fireZone.SetActive(true); // Activate fire zone of player
        SP3.color = new Color(SP3.color.r, SP3.color.g, SP3.color.b, 0.5f); // Set opacity to 50%

        yield return new WaitForSeconds(cooldown);

        fireZone.SetActive(false);

        yield return new WaitForSeconds(spell3CD); // cooldown after use
        
        SP3.color = new Color(SP3.color.r, SP3.color.g, SP3.color.b, 1f); // Restore opacity
        spell3Ready = true;
    }

    // Clean when obkect is destroyed
    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Gameplay.Attack.started -= OnStartShoot;
            controls.Gameplay.Attack.canceled -= OnStopShoot;
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