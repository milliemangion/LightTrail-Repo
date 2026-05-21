using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Scene References")]
    public Transform ground;
    public Transform ceiling;

    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject tokenPrefab;

    public static bool stopSpawning = false;

    [Header("Difficulty")]
    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.8f;

    [Header("Colours")]
    public Color groundColor;
    public Color ceilingColor;
    public Color middleColor = Color.white;

    private float _timer;
    private float _speedTimer;

    private float _lastLane = 0f;
    private bool _canSpawn = true;

    void Update()
    {
        // Stop everything after death
        if (stopSpawning)
            return;

        _timer += Time.deltaTime;

        if (_timer >= spawnRate && _canSpawn)
        {
            StartCoroutine(SpawnPattern());
            _timer = 0f;
        }

        // Difficulty scaling
        _speedTimer += Time.deltaTime;

        if (_speedTimer >= 25f)
        {
            if (ObstacleMover.globalSpeed < 12f)
            {
                ObstacleMover.globalSpeed += 0.05f;
            }

            _speedTimer = 0f;
        }

        // Spawn rate scaling
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= Time.deltaTime * 0.02f;
        }
    }

    IEnumerator SpawnPattern()
    {
        _canSpawn = false;

        int pattern = Random.Range(0, 3);

        // Single obstacle
        if (pattern == 0)
        {
            float lane = GetSafeLane();

            SpawnObstacle(lane);
            SpawnTokenSafe(lane);
        }

        // Double obstacle
        else if (pattern == 1)
        {
            float firstLane = GetSafeLane();

            SpawnObstacle(firstLane);

            yield return new WaitForSeconds(0.5f);

            float secondLane = DifferentLane(firstLane);

            SpawnObstacle(secondLane);
            SpawnTokenSafe(secondLane);
        }

        // Middle obstacle pattern
        else
        {
            SpawnObstacle(MiddleLane());
            SpawnTokenSafe(MiddleLane());
        }

        yield return new WaitForSeconds(0.4f);

        _canSpawn = true;
    }

    float GetSafeLane()
    {
        float lane;

        if (Random.value > 0.6f)
        {
            lane = DifferentLane(_lastLane);
        }
        else
        {
            lane = RandomLane();
        }

        _lastLane = lane;

        return lane;
    }

    float RandomLane()
    {
        int lane = Random.Range(0, 3);

        if (lane == 0)
            return GroundLane();

        if (lane == 1)
            return MiddleLane();

        return CeilingLane();
    }

    float DifferentLane(float currentLane)
    {
        float newLane = RandomLane();

        while (Mathf.Abs(newLane - currentLane) < 0.1f)
        {
            newLane = RandomLane();
        }

        return newLane;
    }

    float GroundLane()
    {
        return ground.position.y + 1f;
    }

    float CeilingLane()
    {
        return ceiling.position.y - 1f;
    }

    float MiddleLane()
    {
        return 0f;
    }

    void SpawnObstacle(float y)
    {
        Vector3 pos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

        float width = Random.Range(0.6f, 1.4f);
        float height = Random.Range(1f, 1.8f);

        obstacle.transform.localScale = new Vector3(width, height, 1f);

        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            if (Mathf.Abs(y - GroundLane()) < 0.1f)
            {
                sr.color = groundColor;
            }
            else if (Mathf.Abs(y - CeilingLane()) < 0.1f)
            {
                sr.color = ceilingColor;
            }
            else
            {
                // Randomly use ground or ceiling colour
                sr.color = Random.value > 0.5f ? groundColor : ceilingColor;
            }
        }
    }

    void SpawnTokenSafe(float obstacleLane)
    {
        float y = 0f;

        // Safe positions AWAY from platforms
        float lowerSafe = ground.position.y + 3f;
        float upperSafe = ceiling.position.y - 3f;

        int choice = Random.Range(0, 3);

        // Middle area
        if (choice == 0)
        {
            y = 0f;
        }

        // Upper air space
        else if (choice == 1)
        {
            y = Random.Range(1.5f, upperSafe);
        }

        // Lower air space
        else
        {
            y = Random.Range(lowerSafe, -1.5f);
        }

        // Prevent token spawning on same lane as obstacle
        if (Mathf.Abs(y - obstacleLane) < 1.5f)
        {
            y = 0f;
        }

        float xOffset = Random.Range(12f, 14f);

        Vector3 pos = new Vector3(xOffset, y, 0f);

        // Extra overlap protection
        Collider2D hit = Physics2D.OverlapCircle(pos, 0.8f);

        if (hit == null)
        {
            Instantiate(tokenPrefab, pos, Quaternion.identity);
        }
    }
}