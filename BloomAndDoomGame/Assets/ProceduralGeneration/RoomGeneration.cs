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

    private void Awake()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        StartCoroutine(GenererDonjon());
    }
    
    private IEnumerator GenererDonjon()
    {
        for (int i = 1; i <= Level; i++)
        {
            if (i == 1)
            {
                Instantiate(RoomCollection.EntryRoom, Vector3.zero, Quaternion.identity);
            }
            else
            {
                GenerateBossRoom();
            }
            yield return new WaitForSeconds(2f);
        }
        BossRooms.Add(Rooms[^1]);
        navMesh.BuildNavMesh();
        Vector3[] positions = new[]
        {
            Vector3.back * 10,
            Vector3.forward * 10,
            Vector3.left * 10,
            Vector3.right * 10, 
        };
        GameManager.Instance.InstantiateMonsters(positions);
    }

    private void GenerateBossRoom()
    {
        GameObject lastRoom = Rooms[^1].gameObject;
        Vector3 position = lastRoom.transform.position;
        Destroy(lastRoom);
        lastRoom = Instantiate(RoomCollection.EntryRoom, position, Quaternion.identity);
        Rooms[^1] = lastRoom.GetComponent<RoomSpawner>();
        BossRooms.Add(Rooms[^1]);
    }

    public void AddRoom(RoomSpawner room)
    {
        Rooms.Add(room);
    }
}
