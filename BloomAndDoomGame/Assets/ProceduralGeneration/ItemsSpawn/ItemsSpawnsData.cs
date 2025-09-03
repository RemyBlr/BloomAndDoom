using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsSpawnsData", menuName = "Scriptable Objects/ItemsSpawnsData")]
public class ItemsSpawnsData : ScriptableObject
{
    public GameObject[] Items;
    public Vector2Int MinMax;

    public void SpawnObjects(List<Vector3> cellsCenter)
    {
        if (Items == null || Items.Length == 0) return;
        int max = Mathf.Min(cellsCenter.Count, MinMax.y);
        int itemCount = Random.Range(MinMax.x, max);
        List<Vector3> availableSpots = new List<Vector3>(cellsCenter);
        for (int i = 0; i < itemCount; i++)
        {
            int index = Random.Range(0, availableSpots.Count);
            Vector3 position = availableSpots[index];
            Instantiate(Items[Random.Range(0, Items.Length)], position, Quaternion.identity);
            availableSpots.RemoveAt(index);
            if (availableSpots.Count == 0) return;
        }
    }
}
