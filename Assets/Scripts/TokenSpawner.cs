using UnityEngine;

public class TokenSpawner : MonoBehaviour
{
    public GameObject tokenPrefab;
    public float spawnRate = 1.5f;
    public float xRange = 4f;
    public float spawnY = 10f;

    void Start()
    {
        InvokeRepeating("SpawnToken", 1f, spawnRate);
    }

    void SpawnToken()
    {
        float randomX = Random.Range(-xRange, xRange);

        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

        Instantiate(tokenPrefab, spawnPos, Quaternion.identity);
    }
}