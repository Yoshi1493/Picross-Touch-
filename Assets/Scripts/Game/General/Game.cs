using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameSettings;

public class Game : MonoBehaviour
{
    [SerializeField] Transform cellHolder;
    Cell[,] cells;

    public Stack<CellType[,]> undoStack = new Stack<CellType[,]>();
    public Stack<CellType[,]> redoStack = new Stack<CellType[,]>();

    public event System.Action<CellData> UpdateBoardAction;
    public event System.Action GameOverAction;

    void Awake()
    {
        UpdateBoardAction += AutofillEmptyCells;
        UpdateBoardAction += CheckGameOver;
        GameOverAction += SavePuzzle;

        GetComponent<InputHandler>().HighlightEndAction += SetCells;
        FindObjectOfType<HintHandler>().PerformHintAction += FillHintCells;

        //init. based on loaded puzzle's row/column counts
        cells = new Cell[targetPuzzleData.RowCount, targetPuzzleData.ColCount];
    }

    void Start()
    {
        //init. cell gameobject array
        for (int row = 0; row < targetPuzzleData.RowCount; row++)
        {
            for (int col = 0; col < targetPuzzleData.ColCount; col++)
            {
                cells[row, col] = cellHolder.GetChild(row * targetPuzzleData.ColCount + col).GetComponent<Cell>();
                SetCellType(row, col, currentPuzzleData.cellData.Cells[row, col]);
            }
        }

        InitEmptyRowsAndColumns();

        //reset current input tool
        currentInputTool = InputTool.Pencil;
    }

    //if the puzzle's solution contains a fully empty row or column, automatically cross out all cells in it
    //do this only if player has autofill enabled in settings
    void InitEmptyRowsAndColumns()
    {
        if (!playerSettings.autofillEnabled) return;

        for (int row = 0; row < targetPuzzleData.RowCount; row++)
        {
            if (targetPuzzleData.cellData.RowData[row].Count == 0)
            {
                for (int col = 0; col < targetPuzzleData.ColCount; col++)
                {
                    SetCellType(row, col, CellType.Crossed);
                }
            }
        }

        for (int col = 0; col < targetPuzzleData.ColCount; col++)
        {
            if (targetPuzzleData.cellData.ColData[col].Count == 0)
            {
                for (int row = 0; row < targetPuzzleData.RowCount; row++)
                {
                    SetCellType(row, col, CellType.Crossed);
                }
            }
        }
    }

    void SetCells(List<Vector2Int> selectedCells)
    {
        //make sure at least 1 cell is selected
        if (selectedCells.Count == 0) return;

        //push current puzzle data
        Do(currentPuzzleData.cellData.Cells.Clone() as CellType[,]);

        //determine whether or not all CellTypes are the same within selectedCells
        bool allSameCellType = false;
        if (selectedCells.Count == 1) allSameCellType = true;
        else allSameCellType = selectedCells.TrueForAll(i => cells[i.x, i.y].CurrentCellType == cells[selectedCells[0].x, selectedCells[0].y].CurrentCellType);

        //set CellType based on current CellType and current input tool
        foreach (var cell in selectedCells)
        {
            switch (currentInputTool)
            {
                case InputTool.Pencil:
                    switch (cells[cell.x, cell.y].CurrentCellType)
                    {
                        case CellType.Empty:
                            SetCellType(cell.x, cell.y, CellType.Filled);
                            break;

                        case CellType.Filled:
                            if (allSameCellType) SetCellType(cell.x, cell.y, CellType.Empty);
                            break;

                        case CellType.Crossed:
                            if (allSameCellType) SetCellType(cell.x, cell.y, CellType.Filled);
                            break;
                    }
                    break;

                case InputTool.Eraser:
                    switch (cells[cell.x, cell.y].CurrentCellType)
                    {
                        case CellType.Empty:
                            SetCellType(cell.x, cell.y, CellType.Crossed);
                            break;

                        case CellType.Filled:
                            SetCellType(cell.x, cell.y, CellType.Crossed);
                            break;

                        case CellType.Crossed:
                            if (allSameCellType) SetCellType(cell.x, cell.y, CellType.Empty);
                            break;
                    }
                    break;
            }
        }

        UpdateRowColumnData(selectedCells);
        UpdateBoardAction.Invoke(currentPuzzleData.cellData);
    }

    //fill in all cells in all elements of hintCellCoordinates
    //elements of hintcellCoordinates are determined by HintHandler
    void FillHintCells(List<Vector2Int> hintCellCoordinates)
    {
        Do(currentPuzzleData.cellData.Cells.Clone() as CellType[,]);

        hintCellCoordinates.ForEach(i =>
        {
            SetCellType(i.x, i.y, CellType.Filled);
        });

        UpdateRowColumnData();
        UpdateBoardAction.Invoke(currentPuzzleData.cellData);
    }

    void SetCellType(int row, int col, CellType cellType)
    {
        //update current puzzle cell data
        currentPuzzleData.cellData.Cells[row, col] = cellType;

        //update cell display on screen
        cells[row, col].SetCellType(cellType);
    }

    void RedrawGameBoard(CellType[,] newBoard)
    {
        //find all mismatched cells between current puzzle data and newBoard
        for (int row = 0; row < targetPuzzleData.RowCount; row++)
        {
            for (int col = 0; col < targetPuzzleData.ColCount; col++)
            {
                if (newBoard[row, col] != currentPuzzleData.cellData.Cells[row, col])
                {
                    //overwrite current puzzle CellType with newBoard's CellType
                    SetCellType(row, col, newBoard[row, col]);
                }
            }
        }
    }

    //update CellData values
    void UpdateRowColumnData(List<Vector2Int> selectedCells = null)
    {
        //find lower and upper x/y bounds in selectedCells to avoid unnecessary looping
        //if selectedCells is null (i.e. this is called from Undo()/Redo()), set bounds to entire board
        int minRow = selectedCells != null ? selectedCells.Select(i => i.x).Min() : 0;
        int maxRow = selectedCells != null ? selectedCells.Select(i => i.x).Max() : targetPuzzleData.RowCount - 1;
        int minCol = selectedCells != null ? selectedCells.Select(i => i.y).Min() : 0;
        int maxCol = selectedCells != null ? selectedCells.Select(i => i.y).Max() : targetPuzzleData.ColCount - 1;

        for (int row = minRow; row <= maxRow; row++)
        {
            currentPuzzleData.cellData.RowData[row].Clear();
            int filledCells = 0;

            for (int col = targetPuzzleData.ColCount - 1; col >= 0; col--)
            {
                if (currentPuzzleData.cellData.Cells[row, col] == CellType.Filled) filledCells++;
                else
                {
                    if (filledCells > 0)
                    {
                        currentPuzzleData.cellData.RowData[row].Add(filledCells);
                        filledCells = 0;
                    }
                }
            }

            if (filledCells > 0) currentPuzzleData.cellData.RowData[row].Add(filledCells);
        }

        for (int c = minCol; c <= maxCol; c++)
        {
            currentPuzzleData.cellData.ColData[c].Clear();
            int filledCells = 0;

            for (int row = targetPuzzleData.RowCount - 1; row >= 0; row--)
            {
                if (currentPuzzleData.cellData.Cells[row, c] == CellType.Filled) filledCells++;
                else
                {
                    if (filledCells > 0)
                    {
                        currentPuzzleData.cellData.ColData[c].Add(filledCells);
                        filledCells = 0;
                    }
                }
            }

            if (filledCells > 0) currentPuzzleData.cellData.ColData[c].Add(filledCells);
        }
    }

    //check to see if any row/column data matches loaded puzzle's respective row/column data
    //if they match, automatically cross out all empty cells in that row/column
    //do this only if player has autofill enabled in settings
    void AutofillEmptyCells(CellData cellData)
    {
        if (!playerSettings.autofillEnabled) return;

        for (int r = 0; r < targetPuzzleData.RowCount; r++)
        {
            if (cellData.RowData[r].SequenceEqual(targetPuzzleData.cellData.RowData[r]))
            {
                for (int c = 0; c < targetPuzzleData.ColCount; c++)
                {
                    if (cellData.Cells[r, c] == CellType.Empty) SetCellType(r, c, CellType.Crossed);
                }
            }
        }

        for (int c = 0; c < targetPuzzleData.ColCount; c++)
        {
            if (cellData.ColData[c].SequenceEqual(targetPuzzleData.cellData.ColData[c]))
            {
                for (int r = 0; r < targetPuzzleData.RowCount; r++)
                {
                    if (cellData.Cells[r, c] == CellType.Empty) SetCellType(r, c, CellType.Crossed);
                }
            }
        }
    }

    //check if current puzzle data matches loaded puzzle data
    void CheckGameOver(CellData cellData)
    {
        for (int r = 0; r < targetPuzzleData.RowCount; r++)
        {
            for (int c = 0; c < targetPuzzleData.ColCount; c++)
            {
                //only necessary to check all filled cells
                if (cellData.Cells[r, c] == CellType.Filled || targetPuzzleData.cellData.Cells[r, c] == CellType.Filled)
                {
                    //compare CellTypes; stop checking once a match isn't found
                    if (cellData.Cells[r, c] != targetPuzzleData.cellData.Cells[r, c])
                    {
                        return;
                    }
                }
            }
        }

        //if all filled cells in game match text file data, call gameover functions
        currentPuzzleData.completionStatus = CompletionStatus.Complete;
        targetPuzzleData.SaveAsCompletedImage();
        GameOverAction?.Invoke();
    }

    #region Game state Functions
    public void Undo()
    {
        redoStack.Push(currentPuzzleData.cellData.Cells.Clone() as CellType[,]);

        RedrawGameBoard(undoStack.Pop());
        UpdateRowColumnData();
        UpdateBoardAction.Invoke(currentPuzzleData.cellData);
    }

    public void Redo()
    {
        undoStack.Push(currentPuzzleData.cellData.Cells.Clone() as CellType[,]);

        RedrawGameBoard(redoStack.Pop());
        UpdateRowColumnData();
        UpdateBoardAction.Invoke(currentPuzzleData.cellData);
    }

    void Do(CellType[,] gameBoard)
    {
        redoStack.Clear();
        undoStack.Push(gameBoard);
    }
    #endregion

    #region Button Functions
    public void Restart()
    {
        for (int r = 0; r < targetPuzzleData.RowCount; r++)
        {
            for (int c = 0; c < targetPuzzleData.ColCount; c++)
            {
                SetCellType(r, c, CellType.Empty);
            }
        }

        undoStack.Clear();
        redoStack.Clear();

        UpdateRowColumnData();
        UpdateBoardAction.Invoke(currentPuzzleData.cellData);
    }

    public void SavePuzzle()
    {
        if (!(undoStack.Count == 0 && redoStack.Count == 0))
        {
            puzzles[playerSettings.selectedDiffculty][playerSettings.selectedPuzzle] = currentPuzzleData;
            FileHandler.SavePuzzles();
        }
    }
    #endregion

    #region DEBUG
    public void FlipPuzzle()
    {
        CellType[,] tempCopy = currentPuzzleData.cellData.Cells.Clone() as CellType[,];

        for (int i = 0; i < currentPuzzleData.RowCount; i++)
        {
            for (int j = 0; j < currentPuzzleData.ColCount; j++)
            {
                tempCopy[i, j] = currentPuzzleData.cellData.Cells[i, currentPuzzleData.ColCount - 1 - j];
            }
        }

        RedrawGameBoard(tempCopy);
    }

    public void InvertPuzzle()
    {
        CellType[,] tempCopy = currentPuzzleData.cellData.Cells.Clone() as CellType[,];

        for (int i = 0; i < currentPuzzleData.RowCount; i++)
        {
            for (int j = 0; j < currentPuzzleData.ColCount; j++)
            {
                tempCopy[i, j] = currentPuzzleData.cellData.Cells[i, j] == CellType.Filled ? CellType.Empty : CellType.Filled;
            }
        }

        RedrawGameBoard(tempCopy);
    }

#if UNITY_EDITOR
    public static void PrintBoard(CellType[,] board)
    {
        //clear console log
        System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.LogEntries").GetMethod("Clear").Invoke(new object(), null);

        for (int r = board.GetLength(0) - 1; r >= 0; r--)
        {
            string output = "";

            for (int c = board.GetLength(1) - 1; c >= 0; c--)
            {
                output += (int)board[r, c] + (c > 0 ? ", " : "");
            }

            print(output);
        }
    }

    public string ConvertCurrentLevelToData()
    {
        string output = "";

        for (int r = targetPuzzleData.RowCount - 1; r >= 0; r--)
        {
            for (int c = targetPuzzleData.ColCount - 1; c >= 0; c--)
            {
                output += (int)currentPuzzleData.cellData.Cells[r, c] == 1 ? 1 : 0;
                if (c != 0) output += " ";
            }
            if (r != 0) output += '\n';
        }

        return output;
    }
#endif
    #endregion
}