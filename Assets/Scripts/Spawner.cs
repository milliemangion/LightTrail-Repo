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
    private bool lastWasTop = false;

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
            float y;

            // Alternate top and bottom for fairness
            if (lastWasTop)
            {
                y = -2f; // bottom
                lastWasTop = false;
            }
            else
            {
                y = 2f; // top
                lastWasTop = true;
            }

            SpawnObstacle(y);

            spawnTokenNext = true; // next spawn = token
        }
        else
        {
            SpawnTokenBetween();
            spawnTokenNext = false;
        }
    }

    void SpawnObstacle(float y)
    {
        Vector3 spawnPos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        float randomWidth = Random.Range(0.5f, 2f);
        float randomHeight = Random.Range(1f, 3f);

        obstacle.transform.localScale = new Vector3(randomWidth, randomHeight, 1f);

        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = (y < 0) ? groundColor : ceilingColor;
        }
    }

    void SpawnTokenBetween()
    {
        // Always spawn token in SAFE SPACE (middle)
        float y = 0f;

        Vector3 tokenPos = new Vector3(10f, y, 0f);

        Instantiate(tokenPrefab, tokenPos, Quaternion.identity);
    }
}