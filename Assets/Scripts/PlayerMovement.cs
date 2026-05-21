using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip jumpSound;
    public AudioClip tokenSound;
    public AudioClip deathSound;

    [Header("Particles")]
    public ParticleSystem trailParticles;
    public GameObject tokenBurstPrefab;

    [Header("Movement")]
    public float gravityScale = 3f;
    public float rotationDuration = 0.15f;

    private Rigidbody2D rb;

    private bool isUpsideDown = false;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = gravityScale;
        }
    }

    void Update()
    {
        if (isDead)
            return;

        // Keep player fixed horizontally
        if (rb != null)
        {
            rb.linearVelocity =
                new Vector2(0f, rb.linearVelocity.y);
        }

        // Gravity flip
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // PLAY JUMP SOUND
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }

            // Emit particles
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

        if (rb != null)
        {
            rb.gravityScale *= -1;
        }

        StopAllCoroutines();

        StartCoroutine(RotatePlayer());
    }

    IEnumerator RotatePlayer()
    {
        float elapsed = 0f;

        float startRotation =
            transform.eulerAngles.z;

        float targetRotation =
            isUpsideDown ? 180f : 0f;

        // Prevent long spinning
        if (Mathf.Abs(startRotation - targetRotation) > 180f)
        {
            if (startRotation > targetRotation)
                targetRotation += 360f;
            else
                startRotation += 360f;
        }

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;

            float zRotation =
                Mathf.Lerp(
                    startRotation,
                    targetRotation,
                    t
                );

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    zRotation
                );

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                targetRotation % 360f
            );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // GAME OVER
        if (other.CompareTag("Obstacle"))
        {
            if (isDead)
                return;

            isDead = true;

            // PLAY DEATH SOUND
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            // Stop movement
            rb.linearVelocity = Vector2.zero;

            // Disable gravity temporarily
            rb.gravityScale = 0f;

            GameManager.instance.GameOver();
        }

        // COLLECT TOKEN
        if (other.CompareTag("Token"))
        {
            // PLAY TOKEN SOUND INSTANTLY
            if (tokenSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    tokenSound,
                    Camera.main.transform.position,
                    1f
                );
            }

            // Spawn particle burst
            if (tokenBurstPrefab != null)
            {
                GameObject burst = Instantiate(
                    tokenBurstPrefab,
                    other.transform.position,
                    Quaternion.identity
                );

                Destroy(burst, 5f);
            }

            // Remove token
            Destroy(other.gameObject);

            // Add score
            GameManager.instance.AddScore(1);
        }
    }
}