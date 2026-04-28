using UnityEngine;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 0.7f;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // smooth ease-out
            t = 1f - Mathf.Pow(1f - t, 4f);

            canvasGroup.alpha = t;

            time += Time.unscaledDeltaTime; // ⭐ IMPORTANT
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}