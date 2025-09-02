using System;
using UnityEngine;
using System.Collections.Generic;

public class DealMeleeDamage : MonoBehaviour
{
    [SerializeField]
    private float m_Damage = 10f;

    [SerializeField]
    private string m_TargetTag = "Player";
    
    [Header("Hit Prevention")]
    [SerializeField] private float hitCooldown = 0.5f;
    
    private Dictionary<GameObject, float> lastHitTimes = new Dictionary<GameObject, float>();
    private bool hasHitThisSwing = false; // Pour éviter multiples hits par swing

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CharacterStats player))
        {
            player.TakeDamage(m_Damage);
        }
    }

    // Méthodes appelées par les animations pour contrôler les dégâts
    public void EnableMeleeDamage()
    {
        GetComponent<Collider>().enabled = true;
        hasHitThisSwing = false; // Reset pour permettre un nouveau hit
        Debug.Log($"Dégâts de mêlée activés sur {gameObject.name}");
    }

    public void DisableMeleeDamage()
    {
        GetComponent<Collider>().enabled = false;
        Debug.Log($"Dégâts de mêlée désactivés sur {gameObject.name}");
    }

    // Nettoyage des références détruites
    void Update()
    {
        var keysToRemove = new List<GameObject>();
        foreach (var kvp in lastHitTimes)
        {
            if (kvp.Key == null)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            lastHitTimes.Remove(key);
        }
    }
}