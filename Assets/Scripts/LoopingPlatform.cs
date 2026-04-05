using UnityEngine;

public class LoopingPlatform : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float resetPositionX = -20f;
    public float startPositionX = 20f;

    void Update()
    {
        // Move left (fake player movement)
        transform.Translate(Vector2.left * ObstacleMover.globalSpeed * Time.deltaTime);

        // Reset position when off screen
        if (transform.position.x < resetPositionX)
        {
            transform.position = new Vector2(startPositionX, transform.position.y);
        }
    }
}