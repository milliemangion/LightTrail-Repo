using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverPanel;

    public GameObject[] grounds;
    public GameObject[] ceilings;
    public Rigidbody2D playerRb;

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

        gameOverPanel.SetActive(false);
        gameOverText.text = "";

        Time.timeScale = 1f;
        Spawner.stopSpawning = false;
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

        // stop spawning immediately
        Spawner.stopSpawning = true;

        // show UI
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nPress R to Restart";

        UIFade fade = gameOverPanel.GetComponent<UIFade>();
        fade.canvasGroup.alpha = 0f;
        fade.FadeIn();

        // start sequence
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // VERY small delay (just enough for frame update)
        yield return new WaitForSeconds(0.05f);

        // remove obstacles + tokens instantly
        ClearScene();

        // remove platforms immediately
        foreach (GameObject g in grounds)
            g.SetActive(false);

        foreach (GameObject c in ceilings)
            c.SetActive(false);

        playerRb.linearVelocity = new Vector2(0, -6f);  
        playerRb.gravityScale = 1.5f;             
        playerRb.angularVelocity = 200f;         

        // let fall play briefly
        yield return new WaitForSeconds(1f);

        // pause game
        Time.timeScale = 0f;
    }

    void ClearScene()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject o in obstacles)
            Destroy(o);

        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        foreach (GameObject t in tokens)
            Destroy(t);
    }
}