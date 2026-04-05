using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize to prevent faster diagonal movement
        movement = movement.normalized;
        
        float scale = 1 + Mathf.Sin(Time.time * 10f) * 0.05f;
        transform.localScale = new Vector3(scale, scale, 1);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * moveSpeed;
    }
}
