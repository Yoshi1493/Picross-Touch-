using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraCropper : MonoBehaviour
{
    Camera mainCam;
    public const float TargetAspectRatio = 9f / 16f;

    void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    //crop screen to fit target aspect ratio
    void Start()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        //if screen aspect and target aspect are close enough, don't update current camera rect
        if (Mathf.Approximately(currentAspectRatio, TargetAspectRatio)) return;

        //if screen aspect is wider than target aspect, add pillarbox
        if (currentAspectRatio > TargetAspectRatio)
        {
            float normalizedWidth = TargetAspectRatio / currentAspectRatio;
            float pillarboxWidth = (1 - normalizedWidth) / 2;

            mainCam.rect = new Rect(pillarboxWidth, 0, normalizedWidth, 1);
        }
        //if screen aspect is narrower than target aspect, add letterbox
        else if (currentAspectRatio < TargetAspectRatio)
        {
            float normalizedHeight = currentAspectRatio / TargetAspectRatio;
            float letterboxHeight = (1 - normalizedHeight) / 2;

            mainCam.rect = new Rect(0, letterboxHeight, 1, normalizedHeight);
        }
    }
}