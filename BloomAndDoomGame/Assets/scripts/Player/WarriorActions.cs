using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * Gère les actions spécifiques (Input actions) de la classe "Warrior"
*/
public class WarriorActions : MonoBehaviour
{
    [Header("Controls")]
    CharacterStats playerStats;
    private AnimationStateController animationState;
    [SerializeField] private GameObject damageZone; // Jump attack spell object

    [Header("Cooldown")]

    [SerializeField] private RawImage UISpell1;
    [SerializeField] private RawImage UISpell2;
    [SerializeField] private RawImage UISpell3;

    // Taunt
    [SerializeField] private float spell1CD = 7f;
    // Jump attack
    [SerializeField] private float spell2CD = 5f;
    // ???
    [SerializeField] private float spell3CD = 10f; // Cooldown duration in seconds

    [SerializeField] private float spell1Duration = 5f;
    [SerializeField] private float spell2Duration = 5f;
    [SerializeField] private float spell3Duration = 5f;

    private bool spell1Ready = true, spell2Ready = true, spell3Ready = true;

    private void Start()
    {
        animationState = GetComponent<AnimationStateController>();
        playerStats = GetComponent<CharacterStats>();
    }

    private void OnAttack(InputValue value)
    {
        if (animationState != null)
            animationState.OnMeleeAttack();
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
        Debug.Log("Using third spell");
        if (!spell3Ready) return;
        StartCoroutine(TriggerSpell3(spell3Duration));
    }

    //
    // Spell activation
    //
    private IEnumerator TriggerSpell1(float cooldown) // Battlecry
    {
        spell1Ready = false;
        animationState.OnSpell1();
        playerStats.SetAttack(playerStats.GetAttack() * 2);

        UISpell1.color = new Color(UISpell1.color.r, UISpell1.color.g, UISpell1.color.b, 0.5f); // Set opacity to 50%
        yield return new WaitForSeconds(spell1CD);
        UISpell1.color = new Color(UISpell1.color.r, UISpell1.color.g, UISpell1.color.b, 1f); // Restore opacity

        playerStats.SetAttack(playerStats.GetAttack() / 2);
        spell1Ready = true;
    }

    private IEnumerator TriggerSpell2(float cooldown) // Jump attack
    {
        spell2Ready = false;

        animationState.OnSpell2();

        UISpell2.color = new Color(UISpell2.color.r, UISpell2.color.g, UISpell2.color.b, 0.5f); // Set opacity to 50%
        yield return new WaitForSeconds(spell2CD);
        UISpell2.color = new Color(UISpell2.color.r, UISpell2.color.g, UISpell2.color.b, 1f); // Restore opacity

        spell2Ready = true;
    }

    private IEnumerator TriggerSpell3(float cooldown) // Quick attack + defense
    {
        spell3Ready = false;

        animationState.OnSpell3();
        playerStats.SetDefense(playerStats.GetDefense() * 2);

        UISpell3.color = new Color(UISpell3.color.r, UISpell3.color.g, UISpell3.color.b, 0.5f); // Set opacity to 50%
        yield return new WaitForSeconds(spell3CD);
        UISpell3.color = new Color(UISpell3.color.r, UISpell3.color.g, UISpell3.color.b, 1f); // Restore opacity

        playerStats.SetDefense(playerStats.GetDefense() / 2);

        spell3Ready = true;
    }

    // In use in Animator Event
    private void EnableDamageZone()
    {
        damageZone.SetActive(true);
    }

    private void DisableDamageZone()
    {
        damageZone.SetActive(false);
    }
}
