using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    public float timeScoreRate = 5f;

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        score += Mathf.RoundToInt(timeScoreRate * Time.deltaTime);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}