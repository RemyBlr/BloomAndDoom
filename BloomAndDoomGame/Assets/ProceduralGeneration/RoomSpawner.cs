using System;
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
}
