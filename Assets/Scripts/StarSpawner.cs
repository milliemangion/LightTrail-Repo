using UnityEngine;
using UnityEngine.UI;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;

    public int starCount = 50;

    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            CreateStar();
        }
    }

    void CreateStar()
    {
        GameObject star =
            Instantiate(starPrefab, transform);

        RectTransform rt =
            star.GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(
            Random.Range(-900f, 900f),
            Random.Range(-500f, 500f)
        );

        float size =
            Random.Range(2f, 6f);

        rt.sizeDelta =
            new Vector2(size, size);

        star.AddComponent<StarMovement>();
    }
}