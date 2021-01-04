using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using static GameSettings;

public class LevelSelectMenu : Menu
{
    [SerializeField] CameraController cameraController;

    [SerializeField] GameObject[] levelSelectScreens = new GameObject[DifficultyCount];

    //0 = diff up; 1 = diff down
    [SerializeField] GameObject[] boardSizeChangeButtons = new GameObject[2];
    [SerializeField] TextMeshProUGUI boardSizeText;

    [SerializeField] LevelConfirmMenu confirmSelectMenu;

    protected override void Awake()
    {
        base.Awake();
        InitPuzzles();

        if (cameraController.currentScreen == CameraController.CurrentScreen.LevelSelect)
        {
            Enable();
        }
    }

    //add new Picross object for every level select icon in all elements of levelSelectScreens
    void InitPuzzles()
    {
        for (int i = 0; i < DifficultyCount; i++)
        {
            int levelCount = levelSelectScreens[i].transform.childCount;

            if (puzzles[i].Count == 0)
            {
                for (int j = 0; j < levelCount; j++)
                {
                    puzzles[i].Add(new Picross());
                }
            }
            else
            {
                if (puzzles[i].Count == levelCount) continue;

                for (int j = puzzles[i].Count; j < levelCount; j++)
                {
                    puzzles[i].Add(new Picross());
                }
            }
        }

        FileHandler.SavePuzzles();
    }

    void Start()
    {
        UpdateUIElements();
        UpdateLevelSelectButtons();
    }

    void UpdateUIElements()
    {
        //update text based on difficulty
        boardSizeText.text = $"{(playerSettings.selectedDiffculty + 1) * 5}x{(playerSettings.selectedDiffculty + 1) * 5}";

        //hide respective board size change button if difficulty is outside the range of [minBoardSize, maxBoardSize]
        boardSizeChangeButtons[0].SetActive(playerSettings.selectedDiffculty > 0);
        boardSizeChangeButtons[1].SetActive(playerSettings.selectedDiffculty < DifficultyCount - 1);

        //display level select screen based on player settings
        for (int i = 0; i < levelSelectScreens.Length; i++)
        {
            levelSelectScreens[i].SetActive(i == playerSettings.selectedDiffculty);
        }
    }

    void UpdateLevelSelectButtons()
    {
        for (int i = 0; i < DifficultyCount; i++)
        {
            for (int j = 0; j < puzzles[i].Count; j++)
            {
                levelSelectScreens[i].transform.GetChild(j).GetComponent<LevelSelectButton>().UpdateDisplay(puzzles[i][j]);
            }
        }
    }

    public void OnChangeDifficulty(int amount)
    {
        //update last selected board size
        playerSettings.selectedDiffculty = Mathf.Clamp(playerSettings.selectedDiffculty + amount, 0, DifficultyCount - 1);
        UpdateUIElements();
    }

    public void LoadPuzzle()
    {
        //set player settings' current puzzle index to the sibling index of the level icon that was clicked in the scene
        GameObject selectedLevelButton = EventSystem.current.currentSelectedGameObject;
        playerSettings.selectedPuzzle = selectedLevelButton.transform.GetSiblingIndex();

        //parse level data; set target puzzle data to parsed data
        targetPuzzleData = selectedLevelButton.GetComponent<LevelSelectButton>().data.ConvertToPicross();

        //set current puzzle data to puzzles at indexes based on player settings
        currentPuzzleData = puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle];

        //if selected puzzle does not have save data, update current puzzle data to new Picross object
        if (currentPuzzleData.cellData.Cells == null)
        {
            currentPuzzleData = new Picross(targetPuzzleData.name, new CellType[targetPuzzleData.RowCount, targetPuzzleData.ColCount]);
        }

        //if selected puzzle is already complete, open confirm select menu
        if (currentPuzzleData.completionStatus == CompletionStatus.Complete)
        {
            confirmSelectMenu.Open();
            confirmSelectMenu.confirmButton.onClick.AddListener(() => ResetLevel());
            Disable();
        }
        //otherwise go straight into Game scene
        else
        {
            LoadScene(2);
        }
    }

    //reset select puzzle data back to new Picross object
    void ResetLevel()
    {
        puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle] = new Picross(targetPuzzleData.name, new CellType[targetPuzzleData.RowCount, targetPuzzleData.ColCount]);
        currentPuzzleData = puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle];
    }
}