using UnityEngine;
using static GameSettings;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] Transform gridParent;
    [SerializeField] GameObject gridLinePrefab;
    
    [Space]

    [SerializeField] Transform cellParent;
    [SerializeField] GameObject cellPrefab;

    void Awake()
    {
        GenerateGrid();
        GenerateCells();
    }

    //instantiate grid lines based on loaded puzzle's row and column count
    void GenerateGrid()
    {
        for (int i = 1; i < targetPuzzleData.RowCount; i++)
        {
            GameObject newGridLine = Instantiate(gridLinePrefab, gridParent);
            newGridLine.transform.Translate(i * CellScale.y * Vector2.up);
            newGridLine.transform.Rotate(Vector3.forward * 90f);
            
            if (i % 5 == 0) newGridLine.transform.localScale = new Vector3(2, 1, 1);
        }

        for (int i = 1; i < targetPuzzleData.ColCount; i++)
        {
            GameObject newGridLine = Instantiate(gridLinePrefab, gridParent);
            newGridLine.transform.Translate(i * CellScale.x * Vector2.left);
            
            if (i % 5 == 0) newGridLine.transform.localScale = new Vector3(2, 1, 1);
        }
    }

    //instantiate cell prefabs equal to loaded puzzle's (row count * column count), and position them in grid fashion
    void GenerateCells()
    {
        for (int i = 0; i < targetPuzzleData.RowCount; i++)
        {
            for (int j = 0; j < targetPuzzleData.ColCount; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, cellParent);

                //[0, 0] = bottom right cell
                newCell.transform.Translate(Vector3.up * i);
                newCell.transform.Translate(Vector3.left * j);

#if UNITY_EDITOR
                //set name for debugging purposes
                newCell.name = $"Cell[{i}][{j}]";
#endif
            }
        }

        //resize cell container to fit cells on game board
        cellParent.localScale = CellScale;
    }
}