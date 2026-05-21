using UnityEngine;

public class StarMovement : MonoBehaviour
{
    RectTransform rt;

    float speed;

    void Start()
    {
        rt = GetComponent<RectTransform>();

        speed = Random.Range(20f, 100f);
    }

    void Update()
    {
        rt.anchoredPosition +=
            Vector2.left * speed * Time.deltaTime;

        if (rt.anchoredPosition.x < -1000)
        {
            rt.anchoredPosition =
                new Vector2(
                    1000,
                    Random.Range(-500f, 500f)
                );
        }
    }
}