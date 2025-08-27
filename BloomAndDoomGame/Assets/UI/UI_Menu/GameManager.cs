using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager instance;
    
    public GameObject character;

    public GameObject[] Monsters;
    
    private void Awake()
    {
        instance = this;
        Instance = instance;
        DontDestroyOnLoad(this);
    }

    public void InstantiatePlayer()
    {
        Instantiate(character, Vector3.up, Quaternion.identity);
    }

    public void InstantiateMonsters(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int monsterId = Random.Range(0, Monsters.Length);
            Instantiate(Monsters[monsterId], positions[i], Quaternion.identity);
        }
    }
}
