using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected string collector = "Player";
    protected CharacterStats playerStats;
    protected float price;

    public void Start()
    {
        GameObject player = GameObject.FindWithTag(collector);
        if (player != null)
            playerStats = player.GetComponent<CharacterStats>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collector))
        {
            
        }
    }
}