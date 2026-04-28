using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverPanel; // panel reference

    private int score = 0;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        score = 0;
        isGameOver = false;

        scoreText.text = "Score: 0";

        // ✅ hide UI at start
        if (gameOverText != null)
            gameOverText.text = "";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // ✅ show panel + text
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "GAME OVER\nPress R to Restart";

        Time.timeScale = 0f;
    }
}