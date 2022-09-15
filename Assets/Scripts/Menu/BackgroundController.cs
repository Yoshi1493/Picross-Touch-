using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static CoroutineHelper;

public class BackgroundController : MonoBehaviour
{
    Image blackBackground;
    IEnumerator fadeCoroutine;

    void Awake()
    {
        blackBackground = GetComponentInChildren<Image>();
    }

    void Start()
    {
        fadeCoroutine = FadeBackground(1f, 0f, 1f);
        StartCoroutine(fadeCoroutine);
    }

    public IEnumerator FadeBackground(float startAlpha, float endAlpha, float fadeDuration = 1f)
    {
        if (fadeDuration <= 0) yield break;

        blackBackground.raycastTarget = true;
        float currentLerpTime = 0f;

        Color c = blackBackground.color;
        c.a = startAlpha;
        blackBackground.color = c;
        SetRaycastTarget();

        while (blackBackground.color.a != endAlpha)
        {
            float lerpProgress = currentLerpTime / fadeDuration;

            c.a = Mathf.Lerp(startAlpha, endAlpha, lerpProgress);
            blackBackground.color = c;

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        c.a = endAlpha;
        blackBackground.color = c;
        SetRaycastTarget();
    }

    void SetRaycastTarget()
    {
        blackBackground.raycastTarget = blackBackground.color.a > 0;
    }
}