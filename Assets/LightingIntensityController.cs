using System.Collections;
using UnityEngine;

public class LightingIntensityController : MonoBehaviour
{
    public Light environmentLight; // Reference to the environment light
    public float fadeDuration = 60f; // 1 minute in seconds
    public float darkDuration = 780f; // 13 minutes in seconds

    private float initialIntensity;
    private float targetIntensity = 0.1f;

    void Start()
    {
        if (environmentLight == null)
        {
            Debug.LogError("Environment light is not assigned.");
            return;
        }

        initialIntensity = environmentLight.intensity;
        StartCoroutine(ChangeLightingIntensity());
    }

    private IEnumerator ChangeLightingIntensity()
    {
        // Fade from initial intensity to target intensity
        yield return StartCoroutine(FadeIntensity(initialIntensity, targetIntensity, fadeDuration));

        // Stay at target intensity for darkDuration
        yield return new WaitForSeconds(darkDuration);

        // Fade from target intensity back to initial intensity
        yield return StartCoroutine(FadeIntensity(targetIntensity, initialIntensity, fadeDuration));

        // Restart the coroutine to loop the effect
        StartCoroutine(ChangeLightingIntensity());
    }

    private IEnumerator FadeIntensity(float from, float to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            environmentLight.intensity = Mathf.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final intensity is set
        environmentLight.intensity = to;
    }
}
