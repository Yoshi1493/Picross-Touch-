using UnityEngine;
using static GameSettings;

public class AppInit : MonoBehaviour
{
    void Awake()
    {        
        playerSettings.UpdateSettings(FileHandler.LoadSettings());
        FileHandler.LoadPuzzles();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

#if UNITY_STANDALONE
        Screen.fullScreen = false;
#endif
    }
}