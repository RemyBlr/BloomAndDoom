using System;
using UnityEngine;
using UnityEngine.ProBuilder;
public class RoomGeneration : MonoBehaviour
{
    [SerializeField] private GameObject Room;
    [SerializeField] private Vector2Int Size;

    private void Awake()
    {
        if (Room == null) return;
        for (int i = 0; i < 4; i++)
        {
            int y = i / 2;
            int x = i - y * 2;
            Vector3 position = new Vector3(x * Size.x, 0, y * Size.y);
            Instantiate(Room, position, Quaternion.identity);
        }
    }
}
