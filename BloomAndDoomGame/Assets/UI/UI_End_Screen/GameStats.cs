using UnityEngine;
using System.Collections.Generic;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    [Header("Run Data")]
    public float runDuration;
    public float damageDealt;
    public float damageTaken;
    public int goldEarned;
    public int distanceTraveled;

    [Header("Items Collected")]
    public List<string> collectedItems = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
