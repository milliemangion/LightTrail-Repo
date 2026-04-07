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

    private bool spawnTokenNext = false;

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
        if (spawnRate > 0.6f)
        {
            spawnRate -= Time.deltaTime * 0.03f;
        }
    }

    void Spawn()
    {
        if (!spawnTokenNext)
        {
            // 🔴 SPAWN OBSTACLE
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

            spawnTokenNext = true; // next spawn will be token
        }
        else
        {
            // ⭐ SPAWN TOKEN BETWEEN OBSTACLES
            SpawnTokenBetween();
            spawnTokenNext = false; // next spawn will be obstacle
        }
    }

    void SpawnTokenBetween()
    {
        // Place token in safe lane between floor & ceiling
        float y = Random.value > 0.5f ? -1.5f : 1.5f;

        Vector3 tokenPos = new Vector3(10f, y, 0f);

        Instantiate(tokenPrefab, tokenPos, Quaternion.identity);
    }
}