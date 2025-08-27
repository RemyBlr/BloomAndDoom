using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            Instantiate(enemy, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
