using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected string collector = "Player";
    protected CharacterStats playerStats;

    public void Start()
    {
        GameObject player = GameObject.FindWithTag(collector);
        if (player != null)
            playerStats = player.GetComponent<CharacterStats>();
    }
}