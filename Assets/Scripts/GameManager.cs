using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TMP_Text highScoreText;
    public GameObject gameOverPanel;

    [Header("Scene References")]
    public GameObject[] grounds;
    public GameObject[] ceilings;
    public Rigidbody2D playerRb;

    private int score = 0;
    private int highScore = 0;

    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        score = 0;
        isGameOver = false;

        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Score UI
        scoreText.text = "SCORE: 0";

        // Hide BEST text at start
        highScoreText.gameObject.SetActive(false);
        highScoreText.text = "";

        // Hide game over UI
        gameOverPanel.SetActive(false);
        gameOverText.text = "";

        // Reset game state
        Time.timeScale = 1f;
        Spawner.stopSpawning = false;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().name
            );
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
            return;

        score += amount;

        scoreText.text = "SCORE: " + score;

        // Save new high score
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        // Stop spawning
        Spawner.stopSpawning = true;

        // Show game over panel
        gameOverPanel.SetActive(true);

        gameOverText.text =
            "GAME OVER\nPRESS R TO RESTART";

        // Show BEST score ONLY on game over
        highScoreText.gameObject.SetActive(true);
        highScoreText.text =
            "BEST: " + highScore;

        // Fade UI
        UIFade fade = gameOverPanel.GetComponent<UIFade>();

        if (fade != null)
        {
            fade.canvasGroup.alpha = 0f;
            fade.FadeIn();
        }

        // Start death animation
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Small dramatic pause
        yield return new WaitForSeconds(0.05f);

        // Remove obstacles/tokens
        ClearScene();

        // Disable platforms
        foreach (GameObject g in grounds)
        {
            g.SetActive(false);
        }

        foreach (GameObject c in ceilings)
        {
            c.SetActive(false);
        }

        // Dramatic fall
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(0, -6f);

            playerRb.gravityScale = 1.5f;

            playerRb.angularVelocity = 200f;
        }

        // Wait before freeze
        yield return new WaitForSeconds(1f);

        // Freeze game
        Time.timeScale = 0f;
    }

    void ClearScene()
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject o in obstacles)
        {
            Destroy(o);
        }

        GameObject[] tokens =
            GameObject.FindGameObjectsWithTag("Token");

        foreach (GameObject t in tokens)
        {
            Destroy(t);
        }
    }
}