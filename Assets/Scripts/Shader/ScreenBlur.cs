using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static CoroutineHelper;

public class ScreenBlur : MonoBehaviour
{
    [SerializeField] AnimationCurve blurInterpolation;
    [SerializeField] Volume globalVolume;

    DepthOfField dof;
    const float MaxFocusDistance = 3f;
    const float MinFocusDistance = 1f;

    void Awake()
    {
        globalVolume.GetComponentInChildren<Volume>();
        FindObjectOfType<Game>().GameOverAction += OnGameOver;
    }

    void OnGameOver()
    {
        enabled = true;
        StartCoroutine(GradualBlur());
    }

    void OnEnable()
    {
        StartCoroutine(GradualBlur());
    }

    IEnumerator GradualBlur()
    {
        float currentLerpTime = 0;
        float totalLerpTime = 0.5f;

        if (globalVolume.profile.TryGet(out dof))
        {
            while (currentLerpTime < totalLerpTime)
            {
                float lerpProgress = currentLerpTime / totalLerpTime;
                dof.focusDistance.value = Mathf.Lerp(MaxFocusDistance, MinFocusDistance, blurInterpolation.Evaluate(lerpProgress));

                yield return EndOfFrame;
                currentLerpTime += Time.deltaTime;

            }
        }
    }

}