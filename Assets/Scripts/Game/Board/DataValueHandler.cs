using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameSettings;

public class DataValueHandler : MonoBehaviour
{
    HorizontalOrVerticalLayoutGroup rowLayoutGroup, colLayoutGroup;
    TextMeshProUGUI[] rowDataTexts, colDataTexts;

    [SerializeField] Transform rowDataHolder, colDataHolder;
    [SerializeField] GameObject rowDataValuePrefab, colDataValuePrefab;

    CellData targetPuzzleCellData;

    const float CellSize = 16f / 9f * 100;

    const string NormalColour = "FFFFFFFF";
    const string DoubleDigitColour = "FFFF00FF";
    const string NormalFade = "FFFFFF40";
    const string DoubleDigitFade = "FFFF0040";

    void Awake()
    {
        FindObjectOfType<Game>().UpdateBoardAction += UpdateDataValues;

        InitLayoutGroups();
        GenerateDataValues();
    }

    //update row and column layout group spacing+padding based on puzzle dimensions
    void InitLayoutGroups()
    {
        rowLayoutGroup = rowDataHolder.GetComponent<VerticalLayoutGroup>();
        colLayoutGroup = colDataHolder.GetComponent<HorizontalLayoutGroup>();

        rowLayoutGroup.spacing = CellSize * CellScale.y;
        colLayoutGroup.spacing = CellSize * CellScale.x;

        rowLayoutGroup.padding.top = Mathf.RoundToInt(rowLayoutGroup.spacing / 2);
        colLayoutGroup.padding.left = Mathf.RoundToInt(colLayoutGroup.spacing / 2);
    }

    //instantiate data value text prefabs and set text to display respective row/column data
    void GenerateDataValues()
    {
        targetPuzzleCellData = targetPuzzleData.cellData;

        //display respective row data values
        for (int i = targetPuzzleData.RowCount - 1; i >= 0; i--)
        {
            GameObject newRowDataValue = Instantiate(rowDataValuePrefab, rowDataHolder);
            TextMeshProUGUI tmp = newRowDataValue.GetComponent<TextMeshProUGUI>();

            //update font size based on row count
            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.y - 1));

            //if row is empty, print "0"
            if (targetPuzzleCellData.RowData[i].Count == 0)
            {
                tmp.text = "0";
            }
            //otherwise print all values
            else
            {
                foreach (var item in targetPuzzleCellData.RowData[i])
                {
                    tmp.text += $"{ApplyColour(item)} ";
                }
            }
        }

        //repeat for column data values
        for (int i = targetPuzzleData.ColCount - 1; i >= 0; i--)
        {
            GameObject newColDataValue = Instantiate(colDataValuePrefab, colDataHolder);
            TextMeshProUGUI tmp = newColDataValue.GetComponent<TextMeshProUGUI>();

            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.x - 1));

            if (targetPuzzleCellData.ColData[i].Count == 0)
            {
                tmp.text = "0";
            }
            else
            {
                foreach (var item in targetPuzzleCellData.ColData[i])
                {
                    tmp.text += $"{ApplyColour(item)}\n";
                }
            }
        }

        rowDataTexts = rowDataHolder.GetComponentsInChildren<TextMeshProUGUI>();
        colDataTexts = colDataHolder.GetComponentsInChildren<TextMeshProUGUI>();
    }

    //check whether or not each row/column data matches respective row/column data in loaded puzzle
    //if it does, grey out the respective row/column data text
    void UpdateDataValues(CellData cellData)
    {
        //for each row
        for (int r = 0; r < targetPuzzleData.RowCount; r++)
        {
            TextMeshProUGUI rowDataText = rowDataTexts[targetPuzzleData.RowCount - 1 - r];
            string[] textValues = rowDataText.text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<int> currentRowData = cellData.RowData[r];
            List<int> targetRowData = targetPuzzleCellData.RowData[r];
            int minCheck = Mathf.Min(currentRowData.Count, targetRowData.Count);

            List<int> matchedIndexes = new(targetRowData.Count);
            int furthestMatchedIndex = 0;

            //for each number in row's current cell data
            for (int i = 0; i < minCheck; i++)
            {
                int ii = furthestMatchedIndex;

                //check to see if it matches a number in target puzzle's respective row data
                while (ii < targetRowData.Count)
                {
                    //if it does, immediately continue check on next value of current cell data
                    if (currentRowData[i] == targetRowData[ii])
                    {
                        matchedIndexes.Add(ii);
                        furthestMatchedIndex = ii + 1;
                        break;
                    }
                    ii++;
                }
            }

            //for each number in row's target cell data
            for (int i = 0; i < targetRowData.Count; i++)
            {
                if (matchedIndexes.Count > 0)
                {
                    int data = targetRowData[i];

                    //check if current index matches an element in list of matched indexes
                    //since list of matched indexes is already ordered,
                    //just check if it equals the first element in list
                    //if it does, remove from list and continue loop
                    if (i == matchedIndexes[0])
                    {
                        textValues[i] = ApplyFade(data);
                        matchedIndexes.RemoveAt(0);
                        continue;
                    }
                    else
                    {
                        textValues[i] = ApplyColour(data);
                    }
                }
                else
                {
                    while (i < targetRowData.Count)
                    {
                        textValues[i] = ApplyColour(targetRowData[i]);
                        i++;
                    }
                }
            }

            rowDataText.text = string.Join(' ', textValues);
        }

        //repeat for each column
        for (int c = 0; c < targetPuzzleData.ColCount; c++)
        {
            TextMeshProUGUI colDataText = colDataTexts[targetPuzzleData.ColCount - 1 - c];
            string[] textValues = colDataText.text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            List<int> currentColData = cellData.ColData[c];
            List<int> targetColData = targetPuzzleCellData.ColData[c];
            int minCheck = Mathf.Min(currentColData.Count, targetColData.Count);

            List<int> matchedIndexes = new(targetColData.Count);
            int furthestMatchedIndex = 0;

            for (int i = 0; i < minCheck ; i++)
            {
                int ii = furthestMatchedIndex;

                while (ii < targetColData.Count)
                {
                    if (currentColData[i] == targetColData[ii])
                    {
                        matchedIndexes.Add(ii);
                        furthestMatchedIndex = ii + 1;
                        break;
                    }
                    ii++;
                }
            }

            for (int i = 0; i < targetColData.Count; i++)
            {
                if (matchedIndexes.Count > 0)
                {
                    int data = targetColData[i];

                    if (i == matchedIndexes[0])
                    {
                        textValues[i] = ApplyFade(data);
                        matchedIndexes.RemoveAt(0);
                        continue;
                    }
                    else
                    {
                        textValues[i] = ApplyColour(data);
                    }
                }
                else
                {
                    while (i < targetColData.Count)
                    {
                        textValues[i] = ApplyColour(targetColData[i]);
                        i++;
                    }
                }
            }

            colDataText.text = string.Join('\n', textValues);
        }

    }

    //set colour of row/column data values. change colour if <num> is more than 1digit (to differentiate between "11" and "1 1")
    string ApplyColour(int num)
    {
        return $"<color=#{(num >= 10 ? DoubleDigitColour : NormalColour)}>{num}</color>";
    }

    //same as ApplyColour, but based on whether value matches target puzzle data
    string ApplyFade(int num)
    {
        return $"<color=#{(num >= 10 ? DoubleDigitFade : NormalFade)}>{num}</color>";
    }
}