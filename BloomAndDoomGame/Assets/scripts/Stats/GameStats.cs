using UnityEngine;
using System;

[System.Serializable]
public class GameSession
{
    public string characterClassName;

    // Timer
    public float startTime;
    public float endTime;
    public float GetSessionDuration() => endTime - startTime;
    
    // Atk stats
    public float totalDamageDealt;
    public float totalDamageTaken;
    public int enemiesKilled;
    
    // Progress
    public int finalLevel;
    public float distanceTraveled;
    
    // Curreny
    public int currencyGained;
}

//-------------------------------------------------------------------------------------
// This class saves and calculates actions done by the player
// Gives functions that return end game stats for the EndGame panel to show
//-------------------------------------------------------------------------------------
public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
