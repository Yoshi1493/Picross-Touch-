using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameSettings;

public class DataValueHandler : MonoBehaviour
{
    [SerializeField] Transform rowDataHolder, colDataHolder;
    HorizontalOrVerticalLayoutGroup rowLayoutGroup, colLayoutGroup;
    [SerializeField] GameObject rowDataValuePrefab, colDataValuePrefab;

    const float CellSize = 16f / 9f * 100;

    void Awake()
    {
        GenerateDataValues();
        FindObjectOfType<Game>().UpdateBoardAction += UpdateDataValues;

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
        for (int i = targetPuzzleData.RowCount - 1; i >= 0; i--)
        {
            GameObject newRowDataValue = Instantiate(rowDataValuePrefab, rowDataHolder) as GameObject;
            TextMeshProUGUI tmp = newRowDataValue.GetComponent<TextMeshProUGUI>();

            //update font size based on row count
            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.y - 1));

            //display respective row data (print 0
            if (targetPuzzleData.cellData.RowData[i].Count == 0)
            {
                tmp.text = "0";
                tmp.color = Color.grey;
            }
            else
            {
                foreach (var item in targetPuzzleData.cellData.RowData[i])
                {
                    tmp.text += item + " ";
                }
            }
        }

        for (int i = targetPuzzleData.ColCount - 1; i >= 0; i--)
        {
            GameObject newColDataValue = Instantiate(colDataValuePrefab, colDataHolder) as GameObject;
            TextMeshProUGUI tmp = newColDataValue.GetComponent<TextMeshProUGUI>();

            tmp.fontSize = DefaultFontSize - (FontSizeReductionRate * (InverseCellScale.x - 1));

            if (targetPuzzleData.cellData.ColData[i].Count == 0)
            {
                tmp.text = "0";
                tmp.color = Color.grey;
            }
            else
            {
                foreach (var item in targetPuzzleData.cellData.ColData[i])
                {
                    tmp.text += item + "\n";
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
            rowDataHolder.GetChild(targetPuzzleData.RowCount - 1 - r).GetComponent<TextMeshProUGUI>().color = cellData.RowData[r].SequenceEqual(targetPuzzleData.cellData.RowData[r]) ? Color.grey : Color.white;
        }

        for (int c = 0; c < targetPuzzleData.ColCount; c++)
        {
            colDataHolder.GetChild(targetPuzzleData.ColCount - 1 - c).GetComponent<TextMeshProUGUI>().color = cellData.ColData[c].SequenceEqual(targetPuzzleData.cellData.ColData[c]) ? Color.grey : Color.white;
        }
    }
}