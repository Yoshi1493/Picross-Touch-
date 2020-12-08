using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameSettings;
using static Vector2Helper;

public class HighlightDisplay : MonoBehaviour
{
    SpriteRenderer highlightSprite;

    [SerializeField] SpriteRenderer highlightedCellCountBackground;
    TextMeshProUGUI highlightedCellCountText;

    Vector2Int positionOffset = new Vector2Int(1, 1);

    void Awake()
    {
        FindObjectOfType<InputHandler>().HighlightStartAction += OnHighlightStart;
        FindObjectOfType<InputHandler>().HighlightEndAction += OnHighlightEnd;

        highlightSprite = GetComponent<SpriteRenderer>();
        highlightSprite.transform.localScale = CellScale;
        highlightedCellCountText = highlightedCellCountBackground.GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnHighlightStart(Vector2Int start, Vector2Int end)
    {
        //set highlight overlay position
        highlightSprite.transform.position = TransformPoint(start);

        //determine how much to scale the sprite by
        Vector2Int distance = end - start;
        Vector2Int newSize = new Vector2Int(Mathf.Abs(distance.y), Mathf.Abs(distance.x)) + Vector2Int.one;

        if (distance.x < 0)
        {
            newSize.y *= -1;
            highlightSprite.transform.Translate(Vector2.up * CellScale);
        }
        if (distance.y < 0)
        {
            newSize.x *= -1;
            highlightSprite.transform.Translate(Vector2.left * CellScale);
        }

        highlightSprite.size = newSize;
        highlightSprite.enabled = true;

        highlightedCellCountText.text = Mathf.Max(Mathf.Abs(newSize.x), Mathf.Abs(newSize.y)).ToString();
        highlightedCellCountBackground.gameObject.SetActive(true);
        UpdateHighlightedCellCounter(end);
    }

    void OnHighlightEnd(List<Vector2Int> selectedCells)
    {
        highlightSprite.enabled = false;
        highlightedCellCountBackground.gameObject.SetActive(false);
    }

    //update highlightedCellCountBackground position + rotation
    void UpdateHighlightedCellCounter(Vector2Int currentCell)
    {
        //set sprite position to follow the player's touch position
        highlightedCellCountBackground.transform.position = TransformPoint(currentCell + positionOffset);
    }
}