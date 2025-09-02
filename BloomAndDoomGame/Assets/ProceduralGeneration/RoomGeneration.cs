using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.ProBuilder;
public class RoomGeneration : MonoBehaviour
{
    private NavMeshSurface navMesh;
    
    [Min(1)] public int Level = 1;
    
    public RoomCollection RoomCollection;

    public List<RoomSpawner> Rooms = new List<RoomSpawner>(5);
    public List<RoomSpawner> BossRooms = new List<RoomSpawner>();
    
    //Monsters
    public Vector2Int MinMaxMonsters;
    public RoomMonsters[] Monsters;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        StartCoroutine(GenerateDungeon());
    }

    private IEnumerator GenerateDungeon()
    {
        Instantiate(RoomCollection.EntryRoom, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        GameObject lastRoom = Rooms[^1].gameObject;
        Rooms[^1] = lastRoom.GetComponent<RoomSpawner>();
        BossRooms.Add(Rooms[^1]);
        
        navMesh.BuildNavMesh();
        for (int i = 1; i < Rooms.Count - 1; i++)
        {
            Rooms[i].InitialMonsterSpawn(Level);
        }

        if (Monsters[Level - 1].Boss == null) yield return null;
        Rooms[0].SpawnBoss(Monsters[Level - 1].Boss);
    }
    public void AddRoom(RoomSpawner room)
    {
        Rooms.Add(room);
    }
    /*
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || Rooms == null) return;
        for (int i = 1; i < Rooms.Count; i++)
        {
            for (int j = 0; j < Rooms[i].cellsCenter.Count; j++)
            {
                Gizmos.DrawSphere(Rooms[i].cellsCenter[j], 1f);
            }
        }
    }
    */
}
