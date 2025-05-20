using System.Collections;
using UnityEngine;

public class ScaleAndPopEffect : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.3f;

    [SerializeField]
    private float scaleMultiplier = 1.3f;

    [SerializeField]
    private float initialScaleFactor = 0.1f;

    [SerializeField]
    private AnimationCurve easingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField]
    private float startDelay = 0f;

    public System.Action onAnimationComplete;

    public void StartScaleAndPopEffect()
    {
        StartCoroutine(ScaleAndPopCoroutine());
    }

    private IEnumerator ScaleAndPopCoroutine()
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;
        float elapsedTime = 0f;

        // Scale up
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);
            transform.localScale = Vector3.Lerp(originalScale * initialScaleFactor, targetScale, easingCurve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        // Scale down
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, easingCurve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;

        onAnimationComplete?.Invoke();
    }
}