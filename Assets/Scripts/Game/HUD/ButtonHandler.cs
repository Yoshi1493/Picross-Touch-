using UnityEngine;
using UnityEngine.UI;
using static GameSettings;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] Game gameController;

    //0 = fill; 1 = cross
    [SerializeField] Button[] inputToolButtons;

    //0 = undo; 1 = redo
    [SerializeField] Button[] gameStateButtons;

    void Awake()
    {
        gameController.UpdateBoardAction += UpdateGameStateButtons;
        gameController.GameOverAction += OnGameOver;
        FindObjectOfType<PauseHandler>().GamePauseAction += OnPausedStateChanged;
    }

    public void OnSelectInputTool(int index)
    {
        for (int i = 0; i < inputToolButtons.Length; i++)
        {
            inputToolButtons[i].interactable = i != index;      //enable all input tool buttons except for [index]
            currentInputTool = (InputTool)index;
        }
    }

    void UpdateGameStateButtons(CellData cellData)
    {
        gameStateButtons[0].interactable = gameController.undoStack.Count > 0;
        gameStateButtons[1].interactable = gameController.redoStack.Count > 0;
    }

    void OnPausedStateChanged(bool state)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = !state;
    }

    void OnGameOver()
    {
        OnPausedStateChanged(true);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioController.Instance.PlaySound(clip);
    }
}