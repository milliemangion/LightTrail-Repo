using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem trailParticles;
    public GameObject tokenBurstPrefab;

    [Header("Movement")]
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

        // Gravity flip
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Emit flip particles
            if (trailParticles != null)
            {
                trailParticles.Emit(Random.Range(10, 20));
            }

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
        float duration = 0.15f;
        float elapsed = 0f;

        float startRotation = transform.eulerAngles.z;
        float targetRotation = isUpsideDown ? 180f : 0f;

        // Prevent weird long spins
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
        // Game Over
        if (other.CompareTag("Obstacle"))
        {
            GameManager.instance.GameOver();
        }

        // Collect Token
        if (other.CompareTag("Token"))
        {
            // Spawn burst effect
            if (tokenBurstPrefab != null)
            {
                GameObject burst = Instantiate(
                    tokenBurstPrefab,
                    other.transform.position,
                    Quaternion.identity
                );

                Destroy(burst, 5f);
            }

            Destroy(other.gameObject);

            GameManager.instance.AddScore(1);
        }
    }
}