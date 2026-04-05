using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
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

        // Speed increase every 30 seconds
        speedTimer += Time.deltaTime;

        if (speedTimer >= 10f)
        {
            ObstacleMover.globalSpeed += 0.2f;
            speedTimer = 0f;
        }
    }

    void Spawn()
    {
        float y = Random.value > 0.5f ? -2f : 2f;

        Vector3 spawnPos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // Random size
        float randomWidth = Random.Range(0.5f, 2f);
        float randomHeight = Random.Range(1f, 3.5f);

        obstacle.transform.localScale = new Vector3(randomWidth, randomHeight, 1f);

        // Set color based on position
        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            if (y < 0)
                sr.color = groundColor;
            else
                sr.color = ceilingColor;
        }
    }
}