using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameSettings;

public class HintHandler : MonoBehaviour
{
    int hintsRemaining;     //number of hints allowed per puzzle
    int fillCount;          //number of cells to fill in per hint

    float numTargetFilled;

    List<Vector2Int> hintCellCoordinates;
    public event System.Action<List<Vector2Int>> PerformHintAction;
    public event System.Action<string> CannotPerformHintAction;

    Button hintButton;
    TextMeshProUGUI hintsRemainingText;

    void Awake()
    {
        hintButton = GetComponent<Button>();
        hintsRemainingText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        ResetHintsRemaining();
        fillCount = hintsRemaining;
        hintCellCoordinates = new List<Vector2Int>(fillCount);

        //set total number of filled cells in selected puzzle
        numTargetFilled = targetPuzzleData.cellData.RowData.Sum(i => i.Sum());
    }

    //set starting number of hints according to board size
    //also called upon pressing Restart button
    public void ResetHintsRemaining()
    {
        SetHintsRemaining(playerSettings.selectedDiffculty + 1);
    }

    void SetHintsRemaining(int amount)
    {
        hintsRemaining = amount;
        hintsRemainingText.text = $"Hint:\n{amount}";

        if (amount <= 0)
        {
            hintButton.interactable = false;
        }
    }

    public void OnSelectHint()
    {
        if (hintsRemaining > 0)
        {
            if (CanPerformHint())
            {
                PerformHintAction?.Invoke(hintCellCoordinates);
                SetHintsRemaining(hintsRemaining - 1);
            }
            else
            {
                CannotPerformHintAction?.Invoke("The puzzle is almost complete.\nNo more hints!");
            }
        }
        else
        {
            CannotPerformHintAction?.Invoke("No more hints remaining!");
        }
    }

    //check if there are enough unfilled cells to perform a hint
    bool CanPerformHint()
    {
        hintCellCoordinates.Clear();

        List<Vector2Int> unfilledCells = new List<Vector2Int>();

        //get list of unfilled cells' coordinates
        for (int row = 0; row < targetPuzzleData.RowCount; row++)
        {
            for (int col = 0; col < targetPuzzleData.ColCount; col++)
            {
                if (targetPuzzleData.cellData.Cells[row, col] == CellType.Filled &&
                    currentPuzzleData.cellData.Cells[row, col] != CellType.Filled)
                {
                    unfilledCells.Add(new Vector2Int(row, col));
                }
            }
        }

        //if there are enough unfilled cells, get <fillCount> random elements
        if (unfilledCells.Count > fillCount * 2)
        {
            hintCellCoordinates = unfilledCells.OrderBy(i => Random.value).Take(fillCount).ToList();
            return true;
        }
        else return false;
    }
}