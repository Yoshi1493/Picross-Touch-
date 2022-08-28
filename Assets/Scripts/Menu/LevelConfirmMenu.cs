using UnityEngine.UI;

public class LevelConfirmMenu : Menu
{
    public Button confirmButton;

    public void ResetConfirmButton()
    {
        confirmButton.onClick.RemoveAllListeners();
    }
}