using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public event System.Action<bool> GamePauseAction;

    public void SetPausedState(bool state)
    {
        GamePauseAction?.Invoke(state);
    }

#if UNITY_ANDROID
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            SetPausedState(true);
        }
    }
#endif
}