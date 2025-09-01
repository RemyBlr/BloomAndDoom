using UnityEngine;

[CreateAssetMenu(fileName = "RoomMonsters", menuName = "Scriptable Objects/RoomMonsters")]
public class RoomMonsters : ScriptableObject
{
    public GameObject Boss;
    public GameObject[] monsters;
}
