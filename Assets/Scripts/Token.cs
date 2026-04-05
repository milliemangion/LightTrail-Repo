using UnityEngine;

public class Token : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.left * ObstacleMover.globalSpeed * Time.deltaTime;

        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}