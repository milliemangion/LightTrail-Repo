using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject tokenPrefab;

    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.5f;

    public Color groundColor;
    public Color ceilingColor;

    private float _timer;
    private float _speedTimer;

    // Track last lane to prevent unfair repeats
    private float _lastLane = 0f;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnRate)
        {
            SpawnPattern();
            _timer = 0f;
        }

        // Speed scaling
        _speedTimer += Time.deltaTime;

        if (_speedTimer >= 10f)
        {
            ObstacleMover.globalSpeed += 0.12f;
            _speedTimer = 0f;
        }

        // Spawn rate scaling
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= Time.deltaTime * 0.04f;
        }
    }

    void SpawnPattern()
    {
        int pattern = Random.Range(0, 3);

        switch (pattern)
        {
            case 0:
                // SAFE single obstacle
                float lane = GetSafeLane();
                SpawnObstacle(lane);
                SpawnTokenOptional();
                break;

            case 1:
                // STAGGERED (safe)
                StartCoroutine(SpawnStaggeredSafe());
                break;

            case 2:
                // TOKEN opportunity (breathing space)
                SpawnToken();
                break;
        }
    }

    IEnumerator SpawnStaggeredSafe()
    {
        float firstLane = GetSafeLane();
        SpawnObstacle(firstLane);

        yield return new WaitForSeconds(0.35f);

        float secondLane = OppositeLane(firstLane);
        SpawnObstacle(secondLane);

        SpawnTokenOptional();
    }

    float GetSafeLane()
    {
        float lane;

        // Prevent repeating same lane too often (avoids unfair stacking)
        if (Random.value > 0.6f)
        {
            lane = OppositeLane(_lastLane);
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
        return Random.value > 0.5f ? 2f : -2f;
    }

    float OppositeLane(float lane)
    {
        if (lane == 2f) return -2f;
        if (lane == -2f) return 2f;
        return RandomLane();
    }

    void SpawnObstacle(float y)
    {
        Vector3 pos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

        float width = Random.Range(0.6f, 1.8f);
        float height = Random.Range(1f, 2.5f);

        obstacle.transform.localScale = new Vector3(width, height, 1f);

        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = (y < 0) ? groundColor : ceilingColor;
        }
    }

    void SpawnTokenOptional()
    {
        if (Random.value > 0.4f)
        {
            SpawnToken();
        }
    }

    void SpawnToken()
    {
        float y;

        int lane = Random.Range(0, 3);

        if (lane == 0)
            y = -1.5f; // bottom
        else if (lane == 1)
            y = 0f;    // middle
        else
            y = 1.5f;  // top

        Vector3 pos = new Vector3(10f, y, 0f);

        Instantiate(tokenPrefab, pos, Quaternion.identity);
    }
}