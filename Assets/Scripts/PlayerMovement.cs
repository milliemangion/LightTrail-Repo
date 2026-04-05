using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravityScale = 3f;

    private Rigidbody2D rb;
    private bool isUpsideDown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        // Keep player in place horizontally
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Flip gravity on key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipGravity();
        }
    }

    void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;

        rb.gravityScale *= -1;

        // Flip player visually
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y * -1,
            transform.localScale.z
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }

        if (other.CompareTag("Token"))
        {
            Destroy(other.gameObject);
            Debug.Log("Collected Token!");
        }
    }
}