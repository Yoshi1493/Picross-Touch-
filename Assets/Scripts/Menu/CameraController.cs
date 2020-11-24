using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class CameraController : MonoBehaviour
{
    public enum CurrentScreen
    {
        MainMenu,
        LevelSelect
    }

    public CurrentScreen currentScreen { get; private set; }

    Camera mainCam;
    float levelSelectCamPos;

    IEnumerator cameraPanCoroutine;
    AnimationCurve cameraPanInterpolation = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Awake()
    {
        mainCam = Camera.main;
        levelSelectCamPos = CameraCropper.targetAspectRatio * mainCam.orthographicSize * 2;
    }

    void Start()
    {
        if (SceneTracker.PreviousSceneIndex == 0)
        {
            mainCam.transform.position = new Vector3(0, 0, mainCam.transform.position.z);
        }
        else
        {
            mainCam.transform.position = new Vector3(levelSelectCamPos, 0, mainCam.transform.position.z);
            currentScreen = CurrentScreen.LevelSelect;
        }
    }

    #region Button Functions

    public void OnSelectPlay()
    {
        if (cameraPanCoroutine != null) StopCoroutine(cameraPanCoroutine);

        cameraPanCoroutine = PanCameraTo(levelSelectCamPos);
        StartCoroutine(cameraPanCoroutine);

        currentScreen = CurrentScreen.LevelSelect;
    }

    public void OnSelectBackToMainMenu()
    {
        if (cameraPanCoroutine != null) StopCoroutine(cameraPanCoroutine);

        cameraPanCoroutine = PanCameraTo(0);
        StartCoroutine(cameraPanCoroutine);

        currentScreen = CurrentScreen.MainMenu;
    }

    #endregion

    //translate camera's x-position to endPosX
    IEnumerator PanCameraTo(float endPosX)
    {
        float currentLerpTime = 0;
        float totalLerpTime = 0.5f;
        float startPosX = mainCam.transform.position.x;

        while (mainCam.transform.position.x != endPosX)
        {
            yield return EndOfFrame;
            currentLerpTime += Time.deltaTime;

            Vector3 newPos = mainCam.transform.position;
            newPos.x = Mathf.Lerp(startPosX, endPosX, cameraPanInterpolation.Evaluate(currentLerpTime / totalLerpTime));
            mainCam.transform.position = newPos;
        }
    }
}