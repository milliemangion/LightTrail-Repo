using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public static float globalSpeed = 8f; // medium fast start

    void Update()
    {
        transform.position += Vector3.left * globalSpeed * Time.deltaTime;

        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}