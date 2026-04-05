using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", 1f, spawnRate);
    }

    void SpawnObstacle()
    {
        float yPos;

        // Randomly spawn on ground or ceiling
        if (Random.value > 0.5f)
        {
            yPos = -2f; // ground
        }
        else
        {
            yPos = 2f; // ceiling
        }

        Instantiate(obstaclePrefab, new Vector2(transform.position.x, yPos), Quaternion.identity);
    }
}