using System.Collections;
using UnityEngine;
using TMPro;
using static CoroutineHelper;

public class MainMenu : Menu
{
    [SerializeField] CameraController cameraController;

    [Space]

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] AnimationCurve titleAnimationCurve;

    CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();

        if (cameraController.currentScreen == CameraController.CurrentScreen.LevelSelect)
        {
            Disable();
        }
    }

    IEnumerator Start()
    {
        float currentLerpTime = 0;
        float totalLerpTime = 1;

        while (currentLerpTime < totalLerpTime)
        {
            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;

            //animate character spacing
            titleText.characterSpacing = titleAnimationCurve.Evaluate(currentLerpTime / totalLerpTime);

            //animate canvas alpha
            canvasGroup.alpha = Mathf.Lerp(0, 1, currentLerpTime);
        }

        //enable canvas buttons
        canvasGroup.interactable = true;
    }

#if UNITY_ANDROID
    protected override void HandleBackButtonInput()
    {
        AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        androidJavaObject.Call<bool>("moveTaskToBack", true);
    }
#endif
}