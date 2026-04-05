using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrailFollower : MonoBehaviour
{
    public TrailRecorder playerTrail;
    public GameObject gameOverText;

    public float minSpeed = 3f;
    public float maxSpeed = 10f;
    public float maxDistance = 6f;

    private int currentIndex = 0;
    private bool isGameOver = false;

    void Update()
    {
        // Restart when game is over
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        if (playerTrail == null) return;
        if (playerTrail.trailPoints.Count == 0) return;
        if (currentIndex >= playerTrail.trailPoints.Count) return;

        Vector3 target = playerTrail.trailPoints[currentIndex];

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            playerTrail.transform.position
        );

        // Smooth speed scaling
        float t = Mathf.Clamp01(distanceToPlayer / maxDistance);
        float speed = Mathf.Lerp(minSpeed, maxSpeed, t);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentIndex++;
        }

        // Face movement direction
        Vector3 direction = (playerTrail.transform.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");

            isGameOver = true;

            if (gameOverText != null)
                gameOverText.SetActive(true);

            Time.timeScale = 0.2f;
            StartCoroutine(FreezeAfterDelay());
        }
    }

    IEnumerator FreezeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}