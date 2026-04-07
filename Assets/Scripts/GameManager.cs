using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;

    private int score = 0;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Restart on R
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        isGameOver = true;
        scoreText.text += "\nGAME OVER\nPress R to Restart";
        Time.timeScale = 0f;
    }
}