using UnityEngine;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float duration = 0.7f;

    void Awake()
    {
        // Auto-assign if not set
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Start invisible
        canvasGroup.alpha = 0f;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f, 1f));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    IEnumerator FadeRoutine(float start, float end)
    {
        float time = 0f;

        canvasGroup.alpha = start;

        while (time < duration)
        {
            float t = time / duration;

            // Smooth easing (ease-out)
            t = 1f - Mathf.Pow(1f - t, 3f);

            canvasGroup.alpha = Mathf.Lerp(start, end, t);

            // IMPORTANT: works even when Time.timeScale = 0
            time += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}