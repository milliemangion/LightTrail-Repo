using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}