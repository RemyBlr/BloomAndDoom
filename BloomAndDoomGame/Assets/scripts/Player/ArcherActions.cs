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

    [Header("Controls")]

    private PlayerControls controls;
    CharacterStats playerStats;
    private AnimationStateController animationState;

    [SerializeField] private GameObject fireZone;

    [Header("Cooldown")]

    [SerializeField] private float spell1CD = 10f;
    [SerializeField] private float spell2CD = 15f;
    [SerializeField] private float spell3CD = 5f; // Cooldown duration in seconds

    [SerializeField] private float spell1Duration = 5f;
    [SerializeField] private float spell2Duration = 5f;
    [SerializeField] private float spell3Duration = 5f;

    private bool spell1Ready = true, spell2Ready = true, spell3Ready = true;

    private void Start()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Gameplay.Attack.started += (ctx) => animationState.OnStartShoot();
        controls.Gameplay.Attack.canceled += (ctx) => animationState.OnStopShoot();

        animationState = GetComponent<AnimationStateController>();
        playerStats = GetComponent<CharacterStats>();

        animationState.OnShootCallback += FireArrow;
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
        animationState.SetAttackSpeed(2f);
        yield return new WaitForSeconds(cooldown);
        playerStats.SetAttackSpeed(1f); // back to normal
        animationState.SetAttackSpeed(1f);
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
