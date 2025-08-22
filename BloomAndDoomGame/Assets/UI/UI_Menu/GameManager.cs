using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager instance;
    
    public GameObject character;
    
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
}
