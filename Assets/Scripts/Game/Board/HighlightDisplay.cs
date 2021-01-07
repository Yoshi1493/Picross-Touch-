using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameSettings;
using static Vector2Helper;

public class HighlightDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer cellHighlightSprite;
    [SerializeField] SpriteRenderer rowHighlightSprite;
    [SerializeField] SpriteRenderer colHighlightSprite;

    [SerializeField] SpriteRenderer highlightedCellCountBackground;
    TextMeshProUGUI highlightedCellCountText;

    Vector2Int positionOffset = Vector2Int.one;

    void Awake()
    {
        FindObjectOfType<InputHandler>().HighlightStartAction += OnHighlightStart;
        FindObjectOfType<InputHandler>().HighlightEndAction += OnHighlightEnd;

        highlightedCellCountText = highlightedCellCountBackground.GetComponentInChildren<TextMeshProUGUI>();
        
        //init. highlight sprite scale + size to adjust to puzzle size
        cellHighlightSprite.transform.localScale = CellScale;
        rowHighlightSprite.size = new Vector2(DefaultBoardSize.x, CellScale.y);
        colHighlightSprite.size = new Vector2(CellScale.x, DefaultBoardSize.y);
    }

    void OnHighlightStart(Vector2Int start, Vector2Int end)
    {
        //set cell highlight position
        cellHighlightSprite.transform.position = TransformPoint(start);

        //set row/column highlight position
        rowHighlightSprite.transform.position = TransformPoint(new Vector2Int(end.x, targetPuzzleData.ColCount));
        colHighlightSprite.transform.position = TransformPoint(new Vector2Int(targetPuzzleData.RowCount, end.y));

        //determine how much to scale cell highlight sprite by
        Vector2Int distance = end - start;
        Vector2Int newSize = new Vector2Int(Mathf.Abs(distance.y), Mathf.Abs(distance.x)) + Vector2Int.one;

        //adjust for setting pivot at bottom-right
        if (distance.x < 0)
        {
            newSize.y *= -1;
            cellHighlightSprite.transform.Translate(Vector2.up * CellScale);
        }
        if (distance.y < 0)
        {
            newSize.x *= -1;
            cellHighlightSprite.transform.Translate(Vector2.left * CellScale);
        }
        cellHighlightSprite.size = newSize;

        //display highlight sprites
        cellHighlightSprite.enabled = true;
        rowHighlightSprite.enabled = true;
        colHighlightSprite.enabled = true;

        //update + display amount of highlighted cells
        highlightedCellCountText.text = Mathf.Max(Mathf.Abs(newSize.x), Mathf.Abs(newSize.y)).ToString();
        highlightedCellCountBackground.gameObject.SetActive(true);
        UpdateHighlightedCellCounter(end);
    }

    void OnHighlightEnd(List<Vector2Int> selectedCells)
    {
        cellHighlightSprite.enabled = false;
        rowHighlightSprite.enabled = false;
        colHighlightSprite.enabled = false;
        highlightedCellCountBackground.gameObject.SetActive(false);
    }

    //update highlightedCellCountBackground position
    void UpdateHighlightedCellCounter(Vector2Int currentCell)
    {
        //set position to follow the player's touch position
        highlightedCellCountBackground.transform.position = TransformPoint(currentCell + positionOffset);
    }
}