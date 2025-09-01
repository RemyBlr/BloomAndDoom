using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EOpeningDirection
{
    Bottom = 1,
    Top = 2,
    Left = 3,
    Right = 4
}

public class RoomSpawner : MonoBehaviour
{
    private RoomGeneration roomManager;
    
    public List<Vector3> cellsCenter;
    
    public EOpeningDirection[] OpeningDirections;
    
    public RoomCollection RoomCollection => roomManager.RoomCollection;
    public int Size => roomManager.RoomCollection.Size;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomGeneration>();
    }

    private void Start()
    {
        if (roomManager == null) return;
        
        roomManager.AddRoom(this);
        foreach (EOpeningDirection opening in OpeningDirections)
        {
            Vector3 direction = opening switch
            {
                EOpeningDirection.Bottom => Vector3.back,
                EOpeningDirection.Top => Vector3.forward,
                EOpeningDirection.Left => Vector3.left,
                EOpeningDirection.Right => Vector3.right,
            };
            Vector3 position = transform.position + direction * Size;
            bool hit = Physics.CheckSphere(position, 4, 1 << LayerMask.NameToLayer("Terrain"));
            if (hit) continue;
            Spawn(opening, position);
        }
        
        InitializeSpawnPoints();
    }

    private void Spawn(EOpeningDirection openingDirection, Vector3 position)
    {
        int rand = 0;
        switch (openingDirection)
        {
            case EOpeningDirection.Bottom:
                rand = Random.Range(0, RoomCollection.TopRooms.Length);
                Instantiate(RoomCollection.TopRooms[rand], position, Quaternion.identity);
                break;
            case EOpeningDirection.Top:
                rand = Random.Range(0, RoomCollection.BottomRooms.Length);
                Instantiate(RoomCollection.BottomRooms[rand], position, Quaternion.identity);
                break;
            case EOpeningDirection.Left:
                rand = Random.Range(0, RoomCollection.RightRooms.Length);
                Instantiate(RoomCollection.RightRooms[rand], position, Quaternion.identity);
                break;
            case EOpeningDirection.Right:
                rand = Random.Range(0, RoomCollection.LeftRooms.Length);
                Instantiate(RoomCollection.LeftRooms[rand], position, Quaternion.identity);
                break;
        }
    }
    
    public void InitializeSpawnPoints()
    {
        int cellSize = 4;
        int halfCell = cellSize / 2;
        int cellCount = (Size / cellSize) * (Size / cellSize);
        int width = Size / cellSize;
        Vector3 startPosition = transform.position - transform.forward * (Size / 2f) - transform.right * (Size / 2f);
        cellsCenter = new (cellCount - 2*Size - (2 * (Size - 2)));
        
        int count = 0;
        for (int i = 0; i < cellCount; i++)
        {
            int y = i / width;
            int x = i - (y * width);
            if (y == 0 || y == width - 1 || x == 0 || x == width - 1) continue;
            Vector3 offset = new Vector3(x * cellSize + halfCell, 0, y * cellSize + halfCell);
            Vector3 origin = startPosition + offset;
            Ray ray = new Ray(origin + Vector3.up * 128, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit, 256f, 1 << LayerMask.NameToLayer("Terrain"))) continue;
            cellsCenter.Add(hit.point);
            count++;
        }
    }

    public void InitialMonsterSpawn(int level)
    {
        int max = Mathf.Min(cellsCenter.Count, roomManager.MinMaxMonsters.y);
        int monsterCount = Random.Range(roomManager.MinMaxMonsters.x, max);

        List<Vector3> availableSpots = cellsCenter.ToList();
        GameObject[] monsters = roomManager.Monsters[level - 1].monsters;
        for (int i = 0; i < monsterCount; i++)
        {
            int index = Random.Range(0, availableSpots.Count);
            Vector3 position = availableSpots[index];
            
            //SpawnMonster
            Instantiate(monsters[Random.Range(0, monsters.Length)], position, Quaternion.identity);
            
            availableSpots.RemoveAt(index);
            if (availableSpots.Count == 0) return;
        }
    }
    /*
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || cellsCenter == null) return;
        for (int i = 0; i < cellsCenter.Count; i++)
        {
            Gizmos.DrawSphere(cellsCenter[i], 1f);
        }
    }
    */
}
