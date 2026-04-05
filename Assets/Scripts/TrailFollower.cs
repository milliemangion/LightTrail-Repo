using System.Collections.Generic;
using UnityEngine;

public class TrailFollower : MonoBehaviour
{
    public TrailRecorder playerTrail;

    public float minSpeed = 3f;
    public float maxSpeed = 10f;
    public float maxDistance = 6f;

    private int currentIndex = 0;

    void Update()
    {
        if (playerTrail.trailPoints.Count == 0) return;
        if (currentIndex >= playerTrail.trailPoints.Count) return;

        Vector3 target = playerTrail.trailPoints[currentIndex];

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            playerTrail.transform.position
        );

        // Normalize distance (0 → 1)
        float t = Mathf.Clamp01(distanceToPlayer / maxDistance);

        // Smooth speed increase
        float speed = Mathf.Lerp(minSpeed, maxSpeed, t);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentIndex++;
        }
    }
}