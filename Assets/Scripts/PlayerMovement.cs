using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
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
        // Keep player fixed horizontally
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipGravity();
        }
    }

    void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;

        rb.gravityScale *= -1;

        StopAllCoroutines();
        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        float duration = 0.15f; // snappier
        float elapsed = 0f;

        float startRotation = transform.eulerAngles.z;
        float targetRotation = isUpsideDown ? 180f : 0f;

        // Fix rotation wrap (prevents weird spins)
        if (Mathf.Abs(startRotation - targetRotation) > 180f)
        {
            if (startRotation > targetRotation)
                targetRotation += 360f;
            else
                startRotation += 360f;
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float z = Mathf.Lerp(startRotation, targetRotation, t);
            transform.rotation = Quaternion.Euler(0, 0, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, targetRotation % 360f);
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