using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected string collector = "Player";
    protected CharacterStats playerStats;

    public void OnAwake()
    {
        GameObject player = GameObject.FindWithTag(collector);
        if (player != null)
            playerStats = player.GetComponent<CharacterStats>();
    }
}