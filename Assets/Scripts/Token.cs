using UnityEngine;

public class Token : MonoBehaviour
{
    public int value = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(value);
            Destroy(gameObject);
        }
    }
}