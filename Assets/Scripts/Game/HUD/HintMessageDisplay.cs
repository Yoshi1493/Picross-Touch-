using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class HintMessageDisplay : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [SerializeField] RectTransform popupObject;
    [SerializeField] TextMeshProUGUI messageText;

    [Space]

    [SerializeField] AnimationCurve moveInterpolation;
    [SerializeField] AnimationCurve fadeInInterpolation;
    [SerializeField] AnimationCurve fadeOutInterpolation;

    IEnumerator popupCoroutine;
    const float FadeInDuration = 0.5f;
    const float FadeOutDuration = 1f;
    const float PopupStayDuration = 1.5f;

    [Space]

    [SerializeField] AudioClip errorSfx;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        FindObjectOfType<HintHandler>().CannotPerformHintAction += OnCannotPerformHint;
    }

    void OnCannotPerformHint(string message)
    {
        messageText.text = message;

        if (popupCoroutine == null)
        {
            popupCoroutine = PopupMessage();
            StartCoroutine(popupCoroutine);
        }
    }

    IEnumerator PopupMessage()
    {
        float currentLerpTime = 0f;
        float lerpProgress;

        AudioController.Instance.PlaySound(errorSfx);

        while (currentLerpTime < FadeInDuration)
        {
            lerpProgress = currentLerpTime / FadeInDuration;

            popupObject.anchoredPosition = moveInterpolation.Evaluate(lerpProgress) * Vector2.up;
            canvasGroup.alpha = fadeInInterpolation.Evaluate(lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        yield return WaitForSeconds(PopupStayDuration);
        currentLerpTime = 0f;

        while (currentLerpTime < FadeOutDuration)
        {
            lerpProgress = currentLerpTime / FadeOutDuration;

            canvasGroup.alpha = fadeOutInterpolation.Evaluate(lerpProgress);

            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;
        }

        popupCoroutine = null;
    }
}