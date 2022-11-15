using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using static GameSettings;

public class LevelSelectMenu : Menu
{
    [SerializeField] CameraController cameraController;

    [SerializeField] RectTransform[] levelSelectScreens = new RectTransform[DifficultyCount];
    readonly (float x, float y)[] levelSelectScreenPositions = new (float, float)[DifficultyCount];

    //0 = diff up; 1 = diff down
    [SerializeField] GameObject[] boardSizeChangeButtons = new GameObject[2];
    [SerializeField] TextMeshProUGUI boardSizeText;

    [Space]

    [SerializeField] LevelConfirmMenu confirmSelectMenu;

    [Space]

    [SerializeField] AudioClip selectSfx;
    [SerializeField] AudioClip confirmSelectSfx;

    protected override void Awake()
    {
        base.Awake();
        Initialize();

        if (cameraController.currentScreen == CameraController.CurrentScreen.LevelSelect)
        {
            Enable();
        }
    }

    void Initialize()
    {
        ValidatePuzzles();
        SetLevelSelectScreenPositions();
        FileHandler.SavePuzzles();
    }

    void ValidatePuzzles()
    {
        //for each level select screen
        for (int i = 0; i < levelSelectScreens.Length; i++)
        {
            int currentPuzzleCount = puzzles[i].Count;
            int totalLevelCount = levelSelectScreens[i].transform.childCount;

            //if amount of saved puzzle data objects is less than amount of icons in level select screen
            if (currentPuzzleCount < totalLevelCount)
            {
                for (int _ = currentPuzzleCount; _ < totalLevelCount; _++)
                {
                    puzzles[i].Add(new Picross());
                }
            }

            //confirm match between saved puzzle data and puzzle data in level select menu icon
            for (int ii = 0; ii < totalLevelCount; ii++)
            {
                //only check puzzles that have been opened
                if (puzzles[i][ii].completionStatus != CompletionStatus.Unopened)
                {
                    Picross targetPuzzle = levelSelectScreens[i].GetChild(ii).GetComponent<LevelSelectButton>().data.ConvertToPicross();

                    //if it doesn't match, replace puzzle and clear puzzle data
                    if (puzzles[i][ii].name != targetPuzzle.name)
                    {
                        puzzles[i][ii] = targetPuzzle;
                        puzzles[i][ii].Reset();
                    }
                }
            }
        }

    }

    //set screen positions based on player settings
    void SetLevelSelectScreenPositions()
    {
        //if no saved settings are found,
        if (playerSettings.levelSelectScreenPositions == null)
        {
            //initialize array
            playerSettings.SaveLevelSelectScreenPositions(new (float, float)[DifficultyCount]);

            //set default level select screen positions
            for (int i = 0; i < DifficultyCount; i++)
            {
                levelSelectScreenPositions[i] = (levelSelectScreens[i].anchoredPosition.x, levelSelectScreens[i].anchoredPosition.y);
            }
        }
        else
        {
            //otherwise set level select screen positions to respective saved positions in PlayerSettings
            for (int i = 0; i < DifficultyCount; i++)
            {
                levelSelectScreenPositions[i] = playerSettings.levelSelectScreenPositions[i];
            }
        }
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
            levelSelectScreens[i].gameObject.SetActive(i == playerSettings.selectedDiffculty);
        }
    }

    void UpdateLevelSelectButtons()
    {
        for (int i = 0; i < DifficultyCount; i++)
        {
            //update screen positions
            levelSelectScreens[i].anchoredPosition = new Vector2(levelSelectScreenPositions[i].x, levelSelectScreenPositions[i].y);

            //update icon display based puzzle completion status
            for (int ii = 0; ii < puzzles[i].Count; ii++)
            {
                levelSelectScreens[i].GetChild(ii).GetComponent<LevelSelectButton>().UpdateDisplay(puzzles[i][ii]);
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

        //update puzzle screen positions
        for (int i = 0; i < DifficultyCount; i++)
        {
            levelSelectScreenPositions[i] = (levelSelectScreens[i].anchoredPosition.x, levelSelectScreens[i].anchoredPosition.y);
        }

        playerSettings.SaveLevelSelectScreenPositions(levelSelectScreenPositions);

        //if selected puzzle is already complete, open confirm select menu
        if (currentPuzzleData.completionStatus == CompletionStatus.Complete)
        {
            confirmSelectMenu.Open();
            PlaySound(confirmSelectSfx);

            confirmSelectMenu.confirmButton.onClick.AddListener(() => ResetLevel());
            Disable();
        }
        //otherwise update current puzzle completion status to Incomplete, and go straight to Game scene
        else
        {
            PlaySound(selectSfx);
            LoadSceneAfterDelay(2);
        }
    }

    //reset select puzzle data back to new Picross object
    void ResetLevel()
    {
        puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle] = new Picross(targetPuzzleData.name, new CellType[targetPuzzleData.RowCount, targetPuzzleData.ColCount]);
        currentPuzzleData = puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle];
    }
}