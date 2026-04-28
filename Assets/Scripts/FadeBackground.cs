using UnityEngine;
using System.Collections;

public class MenuIntro : MonoBehaviour
{
    public CanvasGroup fadeOverlay;
    public RectTransform title;
    public GameObject startButton;

    public float fadeDuration = 1.5f;

    void Start()
    {
        startButton.SetActive(false);
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Fade from black
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            // Smooth easing (important)
            float eased = 1 - Mathf.Pow(1 - progress, 3);

            fadeOverlay.alpha = 1 - eased;

            yield return null;
        }

        fadeOverlay.alpha = 0;

        // Title zoom effect
        yield return StartCoroutine(TitleZoom());

        // Show button after delay
        yield return new WaitForSeconds(0.3f);
        startButton.SetActive(true);
    }

    IEnumerator TitleZoom()
    {
        float duration = 1f;
        float t = 0;

        Vector3 startScale = Vector3.one * 0.9f;
        Vector3 endScale = Vector3.one;

        title.localScale = startScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            float eased = 1 - Mathf.Pow(1 - progress, 3);

            title.localScale = Vector3.Lerp(startScale, endScale, eased);

            yield return null;
        }

        title.localScale = endScale;
    }
}