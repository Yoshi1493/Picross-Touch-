using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FileHandler;
using static GameSettings;

public class LevelSelectButton : MonoBehaviour
{
    public TextAsset data;

    [SerializeField] Image puzzleImage;
    [SerializeField] TextMeshProUGUI puzzleName;

    public void UpdateDisplay(Picross puzzle)
    {
        UpdateCompletedImage(puzzle);

        switch (puzzle.completionStatus)
        {
            case CompletionStatus.Unopened:
                break;

            case CompletionStatus.Incomplete:
                break;

            case CompletionStatus.Complete:
                puzzleImage.enabled = true;
                puzzleName.text = puzzle.name;
                break;
        }
    }
    
    void UpdateCompletedImage(Picross puzzle)
    {
        byte[] texData = LoadCompletedImage(puzzle);
        if (texData == null) return;

        Texture2D tex2D = new Texture2D(puzzle.ColCount, puzzle.RowCount)
        {
            filterMode = FilterMode.Point
        };

        tex2D.LoadImage(texData);

        Sprite spr = Sprite.Create(
            tex2D,
            new Rect(0, 0, tex2D.width, tex2D.height),
            Vector2.zero / 2f,
            Mathf.Min(tex2D.width, tex2D.height)
            );

        puzzleImage.sprite = spr;
    }
}