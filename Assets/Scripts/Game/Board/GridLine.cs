using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class GridLine : MonoBehaviour
{
    [SerializeField] AnimationCurve scaleInterpolation;
    public float endScale = 1;

    IEnumerator Start()
    {
        float currentLerpTime = 0;
        float totalLerpTime = 1 / endScale;

        while (transform.localScale.y <= endScale)
        {
            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;

            Vector3 newScale = transform.localScale;
            newScale.y = Mathf.Lerp(0, endScale, scaleInterpolation.Evaluate(currentLerpTime / totalLerpTime));
            transform.localScale = newScale;
        }
    }
}