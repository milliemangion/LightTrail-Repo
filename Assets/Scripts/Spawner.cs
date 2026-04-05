using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject tokenPrefab;
    public float spawnRate = 1.5f;

    public Color groundColor;
    public Color ceilingColor;

    private float timer = 0f;
    private float speedTimer = 0f;

    void Update()
    {
        // Spawn timer
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            Spawn();
            timer = 0f;
        }

        // Speed increase every 10 seconds
        speedTimer += Time.deltaTime;

        if (speedTimer >= 10f)
        {
            ObstacleMover.globalSpeed += 0.1f;
            speedTimer = 0f;
        }

        // Increase spawn frequency over time
        if (spawnRate > 0.6f) // slightly safer minimum
        {
            spawnRate -= Time.deltaTime * 0.03f;
        }
    }

    void Spawn()
    {
        float y = Random.value > 0.5f ? -2f : 2f;

        Vector3 spawnPos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // Random size
        float randomWidth = Random.Range(0.5f, 2f);
        float randomHeight = Random.Range(1f, 3f);

        obstacle.transform.localScale = new Vector3(randomWidth, randomHeight, 1f);

        // Set color
        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = (y < 0) ? groundColor : ceilingColor;
        }

        // Only spawn token sometimes (prevents spam)
        if (Random.value > 0.3f)
        {
            SpawnTokenBetween();
        }
    }

    void SpawnTokenBetween()
    {
        // Slight variation but still safe
        float y = Random.Range(-0.8f, 0.8f);

        Vector3 tokenPos = new Vector3(10f, y, 0f);

        Instantiate(tokenPrefab, tokenPos, Quaternion.identity);
    }
}