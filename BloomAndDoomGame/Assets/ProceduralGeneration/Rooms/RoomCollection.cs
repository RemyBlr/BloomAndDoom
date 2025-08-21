using UnityEngine;

[CreateAssetMenu(fileName = "RoomCollection", menuName = "Scriptable Objects/RoomCollection")]
public class RoomCollection : ScriptableObject
{
    public int Size;
    public GameObject EntryRoom;
    public GameObject[] BottomRooms;
    public GameObject[] TopRooms;
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;
}
