using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Gère les actions spécifiques (Input actions) de la classe "Warrior"
*/
public class WarriorActions : MonoBehaviour
{
    [Header("Controls")]
    private PlayerControls controls;
    CharacterStats playerStats;
    private AnimationStateController animationState;

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

        animationState = GetComponent<AnimationStateController>();
        playerStats = GetComponent<CharacterStats>();
    }

    private void OnAttack(InputValue value)
    {
        if (animationState != null)
            animationState.OnMeleeAttack();
    }

    // private void OnPunch(InputValue value)
    // {
    //     if (animationState != null)
    //         animationState.OnPunch();
    // }

    private void OnSpell_1(InputValue value)
    {
        Debug.Log("Using battlecry on enemies");
        if (!spell1Ready) return;
        StartCoroutine(TriggerSpell1(spell1Duration));
    }

    private void OnSpell_2(InputValue value)
    {
        Debug.Log("Jump attack beware !");
        if (!spell2Ready) return;
        StartCoroutine(TriggerSpell2(spell2Duration));
    }

    private void OnSpell_3(InputValue value)
    {
        Debug.Log("Using third spell");
        if (!spell3Ready) return;
        StartCoroutine(TriggerSpell3(spell3Duration));
    }

    //
    // Spell activation
    //
    private IEnumerator TriggerSpell1(float cooldown)
    {
        spell1Ready = false;

        animationState.OnSpell1();

        yield return new WaitForSeconds(spell1CD);
        spell1Ready = true;
    }

    private IEnumerator TriggerSpell2(float cooldown)
    {
        spell2Ready = false;

        animationState.OnSpell2();

        yield return new WaitForSeconds(spell2CD);
        spell2Ready = true;
    }

    private IEnumerator TriggerSpell3(float cooldown)
    {
        spell3Ready = false;
        yield return new WaitForSeconds(spell3CD);
        spell3Ready = true;
    }
}
