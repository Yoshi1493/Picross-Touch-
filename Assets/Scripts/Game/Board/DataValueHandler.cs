using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameSettings;

public class DataValueHandler : MonoBehaviour
{
    HorizontalOrVerticalLayoutGroup rowLayoutGroup, colLayoutGroup;

    [SerializeField] Transform rowDataHolder, colDataHolder;
    [SerializeField] GameObject rowDataValuePrefab, colDataValuePrefab;

    const float CellSize = 16f / 9f * 100;
    const string DoubleDigitColour = "FFFF00";

    void Awake()
    {
        FindObjectOfType<Game>().UpdateBoardAction += UpdateDataValues;

        rowLayoutGroup = rowDataHolder.GetComponent<VerticalLayoutGroup>();
        colLayoutGroup = colDataHolder.GetComponent<HorizontalLayoutGroup>();

        rowLayoutGroup.spacing = CellSize * CellScale.y;
        colLayoutGroup.spacing = CellSize * CellScale.x;

        rowLayoutGroup.padding.top = Mathf.RoundToInt(rowLayoutGroup.spacing / 2);
        colLayoutGroup.padding.left = Mathf.RoundToInt(colLayoutGroup.spacing / 2);

        GenerateDataValues();
    }

    //instantiate data value text prefabs and set text to display respective row/column data
    void GenerateDataValues()
    {
        //display respective row data values
        for (int i = targetPuzzleData.RowCount - 1; i >= 0; i--)
        {
            GameObject newRowDataValue = Instantiate(rowDataValuePrefab, rowDataHolder) as GameObject;
            TextMeshProUGUI tmp = newRowDataValue.GetComponent<TextMeshProUGUI>();

            //update font size based on row count
            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.y - 1));

            //if row is empty, print "0"
            if (targetPuzzleData.cellData.RowData[i].Count == 0)
            {
                tmp.text = "0";
            }
            //otherwise print all values
            else
            {
                foreach (var item in targetPuzzleData.cellData.RowData[i])
                {
                    //change colour of double digit numbers (to differentiate between "1 1" and "11")
                    if (item >= 10)
                    {
                        tmp.text += $"<color=#{DoubleDigitColour}>{item}</color> ";
                    }
                    else
                    {
                        tmp.text += $"{item} ";
                    }
                }
            }
        }

        //repeat for column data values
        for (int i = targetPuzzleData.ColCount - 1; i >= 0; i--)
        {
            GameObject newColDataValue = Instantiate(colDataValuePrefab, colDataHolder) as GameObject;
            TextMeshProUGUI tmp = newColDataValue.GetComponent<TextMeshProUGUI>();

            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.x - 1));

            if (targetPuzzleData.cellData.ColData[i].Count == 0)
            {
                tmp.text = "0";
            }
            else
            {
                foreach (var item in targetPuzzleData.cellData.ColData[i])
                {
                    if (item >= 10)
                    {
                        tmp.text += $"<color=#{DoubleDigitColour}>{item}</color>\n";
                    }
                    else
                    {
                        tmp.text += $"{item}\n";
                    }
                }
            }
        }

    }

    //check whether or not each row/column data matches respective row/column data in loaded puzzle
    //if it does, grey out the respective row/column data text
    void UpdateDataValues(CellData cellData)
    {
        for (int r = 0; r < targetPuzzleData.RowCount; r++)
        {
            TextMeshProUGUI rowDataText = rowDataHolder.GetChild(targetPuzzleData.RowCount - 1 - r).GetComponent<TextMeshProUGUI>();
            Color rowDataColour = rowDataText.color;

            if (cellData.RowData[r].SequenceEqual(targetPuzzleData.cellData.RowData[r]))
            {
                rowDataColour.a = 0.25f;
            }
            else
            {
                rowDataColour.a = 1;
            }

            rowDataText.color = rowDataColour;
        }

        for (int c = 0; c < targetPuzzleData.ColCount; c++)
        {
            TextMeshProUGUI colDataText = colDataHolder.GetChild(targetPuzzleData.ColCount - 1 - c).GetComponent<TextMeshProUGUI>();
            Color colDataColour = colDataText.color;

            if (cellData.ColData[c].SequenceEqual(targetPuzzleData.cellData.ColData[c]))
            {
                colDataColour.a = 0.25f;
            }
            else
            {
                colDataColour.a = 1;
            }

            colDataText.color = colDataColour;
        }
    }


}