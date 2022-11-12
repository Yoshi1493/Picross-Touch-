using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    #region Enums

    public enum CellType
    {
        Empty,
        Filled,
        Crossed
    }

    public enum InputTool
    {
        Fill,
        Cross
    }

    public enum CompletionStatus
    {
        Unopened,
        Incomplete,
        Complete
    }

    #endregion

    #region Data structures

    [Serializable]
    public struct CellData
    {
        public readonly CellType[,] Cells;
        public readonly List<int>[] RowData, ColData;

        public CellData(CellType[,] cells)
        {
            Cells = cells;
            RowData = new List<int>[cells.GetLength(0)];
            ColData = new List<int>[cells.GetLength(1)];

            InitRowColumnData();
        }

        void InitRowColumnData()
        {
            //for each row (starting from top row)
            for (int r = 0; r < Cells.GetLength(0); r++)
            {
                RowData[r] = new List<int>();
                int filledCells = 0;

                //for each column (starting from leftmost column)
                for (int c = Cells.GetLength(1) - 1; c >= 0; c--)
                {
                    //check for consecutive filled cells in CellData[r]
                    if (Cells[r, c] == CellType.Filled) filledCells++;

                    //upon finding a break, add amount of consecutive filled cells to list
                    else
                    {
                        if (filledCells > 0)
                        {
                            RowData[r].Add(filledCells);
                            filledCells = 0;
                        }
                    }
                }

                if (filledCells > 0) RowData[r].Add(filledCells);
            }

            //repeat for each column
            for (int c = 0; c < Cells.GetLength(1); c++)
            {
                ColData[c] = new List<int>();
                int filledCells = 0;

                for (int r = Cells.GetLength(0) - 1; r >= 0; r--)
                {
                    if (Cells[r, c] == CellType.Filled) filledCells++;
                    else
                    {
                        if (filledCells > 0)
                        {
                            ColData[c].Add(filledCells);
                            filledCells = 0;
                        }
                    }
                }

                if (filledCells > 0) ColData[c].Add(filledCells);
            }

        }
    }

    [Serializable]
    public class Picross
    {
        public string name;
        public CompletionStatus completionStatus;
        public float timeElapsed;

        public CellData cellData;
        public readonly int RowCount, ColCount;

        public Picross(string _name, CellType[,] cells)
        {
            name = _name;

            cellData = new CellData(cells);
            RowCount = cellData.Cells.GetLength(0);
            ColCount = cellData.Cells.GetLength(1);
        }

        public Picross() { }

        public bool IsNotEmpty()
        {
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    if (cellData.Cells[r, c] != CellType.Empty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    #endregion

    #region Variables

    #region Puzzle data

    public const int DifficultyCount = 4;               //5x5, 10x10, 15x15, 20x20

    public static Picross targetPuzzleData;             //target picross data that the player has to match in order to "win"
    public static Picross currentPuzzleData;            //player's current picross data
    public static List<Picross>[] puzzles;              //"save data" - to hold every complete and incomplete (but not unopened) puzzle

    public const string TimeTextFormat = "mm':'ss";     //format Picross.timeElapsed as 00:00

    #endregion

    #region Input data

    public static InputTool currentInputTool;

    #endregion

    #region Game board generation

    public static Vector2 DefaultBoardSize = new(5f, 5f);

    //for scaling certain game board elements relative to DefaultBoardSize
    public static Vector2 CellScale => new(DefaultBoardSize.x / targetPuzzleData.ColCount, DefaultBoardSize.y / targetPuzzleData.RowCount);
    public static Vector2 InverseCellScale => new(1 / CellScale.x, 1 / CellScale.y);

    //row/column font size for a 5x5 level
    public const float DefaultFontSize = 50;

    //decrease font size by this much for every additional 5 rows/columns in the level
    public const float FontSizeReductionRate = 9;

    #endregion

    #region Player settings

    public static PlayerSettings playerSettings = new();

    #endregion

    #endregion
}