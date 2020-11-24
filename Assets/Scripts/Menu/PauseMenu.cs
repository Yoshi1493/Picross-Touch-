using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu
{
    protected override void Awake()
    {
        base.Awake();
        GetComponent<PauseHandler>().GamePauseAction += OnPausedStateChanged;
    }
    
    void OnPausedStateChanged(bool state)
    {
        if (state) Open();
        else Close();
    }
    
#if UNITY_ANDROID
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            GetComponentInChildren<Button>().onClick.Invoke();
        }
    }
#endif
}