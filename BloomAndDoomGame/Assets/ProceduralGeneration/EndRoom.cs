using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndRoom : MonoBehaviour
{
    private RoomGeneration roomGeneration;
    private CharacterStats player;
    private EnemyDamageSystem enemyDamageSystem;
    
    private void Awake()
    {
        roomGeneration = FindFirstObjectByType<RoomGeneration>();
        player = FindFirstObjectByType<CharacterStats>();
        enemyDamageSystem = GetComponent<EnemyDamageSystem>();
    }

    private void Start()
    {
        enemyDamageSystem.OnDeath += OnBossDeath;
    }

    public void OnBossDeath()
    {
        player.transform.localPosition = Vector3.zero;
        SceneManager.LoadScene(roomGeneration.NextRoom);
        FindFirstObjectByType<CharacterStats>().transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
