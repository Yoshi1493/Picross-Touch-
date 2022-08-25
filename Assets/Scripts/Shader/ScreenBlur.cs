using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class ScreenBlur : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] AnimationCurve blurInterpolation;

    const int MaxBlurIterations = 10;
    const float MaxDownresAmount = 2;
    int blurIterations;
    float downresAmount;

    void Awake()
    {
        FindObjectOfType<Game>().GameOverAction += OnGameOver;
    }
    
    void OnGameOver()
    {
        enabled = true;
        StartCoroutine(GradualBlur());
    }

    IEnumerator GradualBlur()
    {
        float currentLerpTime = 0;
        float totalLerpTime = 0.5f;

        while (currentLerpTime < totalLerpTime)
        {
            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;

            float lerpProgress = currentLerpTime / totalLerpTime;
            blurIterations = (int)Mathf.Lerp(0, MaxBlurIterations, blurInterpolation.Evaluate(lerpProgress));
            downresAmount = Mathf.Lerp(0, MaxDownresAmount, blurInterpolation.Evaluate(lerpProgress));
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (mat == null) return;

        int width = (int)(src.width / Mathf.Pow(2, downresAmount));
        int height = (int)(src.height / Mathf.Pow(2, downresAmount));

        RenderTexture rt1 = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(src, rt1);

        for (int i = 0; i < blurIterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt1, rt2, mat);
            RenderTexture.ReleaseTemporary(rt1);
            rt1 = rt2;
        }

        Graphics.Blit(rt1, dst);
        RenderTexture.ReleaseTemporary(rt1);
    }
}