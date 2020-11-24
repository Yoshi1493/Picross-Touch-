using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : Menu
{
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