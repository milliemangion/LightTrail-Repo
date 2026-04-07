using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject tokenPrefab;

    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.8f;

    public Color groundColor;
    public Color ceilingColor;

    private float _timer;
    private float _speedTimer;

    private float _lastLane = 0f;
    private bool _canSpawn = true;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnRate && _canSpawn)
        {
            StartCoroutine(SpawnPattern());
            _timer = 0f;
        }

        // Difficulty timer
        _speedTimer += Time.deltaTime;

        if (_speedTimer >= 25f)
        {
            // Increase difficulty ONLY if below safe limit
            if (ObstacleMover.globalSpeed < 12f)
            {
                ObstacleMover.globalSpeed += 0.05f;
            }
            else
            {
                // If too fast → slightly reduce (prevents impossible state)
                ObstacleMover.globalSpeed -= 0.1f;
            }

            _speedTimer = 0f;
        }

        // Spawn rate control
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= Time.deltaTime * 0.02f;
        }
        else
        {
            // If too fast spawning → ease it slightly
            spawnRate += Time.deltaTime * 0.01f;
        }
    }

    IEnumerator SpawnPattern()
    {
        _canSpawn = false;

        int pattern = Random.Range(0, 2); // fewer patterns = safer

        if (pattern == 0)
        {
            float lane = GetSafeLane();
            SpawnObstacle(lane);
            SpawnTokenSafe(lane);
        }
        else
        {
            float firstLane = GetSafeLane();
            SpawnObstacle(firstLane);

            yield return new WaitForSeconds(0.5f); // ✅ BIGGER GAP

            float secondLane = OppositeLane(firstLane);
            SpawnObstacle(secondLane);

            SpawnTokenSafe(secondLane);
        }

        // ✅ enforce spacing between patterns
        yield return new WaitForSeconds(0.4f);

        _canSpawn = true;
    }

    float GetSafeLane()
    {
        float lane;

        if (Random.value > 0.6f)
            lane = OppositeLane(_lastLane);
        else
            lane = RandomLane();

        _lastLane = lane;
        return lane;
    }

    float RandomLane()
    {
        return Random.value > 0.5f ? 2f : -2f;
    }

    float OppositeLane(float lane)
    {
        return lane == 2f ? -2f : 2f;
    }

    void SpawnObstacle(float y)
    {
        Vector3 pos = new Vector3(10f, y, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

        float width = Random.Range(0.6f, 1.4f);
        float height = Random.Range(1f, 1.8f); // ✅ LOWER HEIGHT = NO BLOCKING

        obstacle.transform.localScale = new Vector3(width, height, 1f);

        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = (y < 0) ? groundColor : ceilingColor;
        }
    }

    void SpawnTokenSafe(float obstacleLane)
    {
        float y;

        // ✅ ALWAYS opposite or middle = never blocked
        int choice = Random.Range(0, 2);

        if (choice == 0)
            y = 0f; // middle safe lane
        else
            y = (obstacleLane == 2f) ? -1.5f : 1.5f;

        // ✅ spawn AFTER obstacle (never overlap)
        float xOffset = Random.Range(12f, 14f);

        Vector3 pos = new Vector3(xOffset, y, 0f);

        Instantiate(tokenPrefab, pos, Quaternion.identity);
    }
    
}