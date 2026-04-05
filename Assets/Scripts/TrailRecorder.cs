using System.Collections.Generic;
using UnityEngine;

public class TrailRecorder : MonoBehaviour
{
    public List<Vector3> trailPoints = new List<Vector3>();
    public float recordDistance = 0.2f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        trailPoints.Add(transform.position);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, lastPosition) > recordDistance)
        {
            trailPoints.Add(transform.position);
            lastPosition = transform.position;
        }
    }
}