using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerInstance;
    public GameObject hudInstance;

    public GameObject[] Monsters;

    [Header("HUD")]
    public GameObject hudPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void InstantiatePlayer()
    {
        if (SelectedCharacter.pickedClass == null) {
            Debug.LogError("Aucune classe sélectionnée");
            return;
        }

        playerInstance = Instantiate(SelectedCharacter.pickedClass.prefab, Vector3.up, Quaternion.identity);

        //if (!playerInstance.CompareTag("Player"))
            //playerInstance.tag = "Player";

        if (hudInstance == null && hudPrefab != null) {
            hudInstance = Instantiate(hudPrefab);
            DontDestroyOnLoad(hudInstance);
        }

        if (GameStats.Instance == null) {
            GameObject statsManager = new GameObject("GameStats");
            statsManager.AddComponent<GameStats>();
        }
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