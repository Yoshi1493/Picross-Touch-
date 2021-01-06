using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FileHandler;
using static GameSettings;

public class LevelSelectButton : MonoBehaviour
{
    public TextAsset data;

    [SerializeField] Image[] puzzleIcons = new Image[System.Enum.GetNames(typeof(CompletionStatus)).Length];
    [SerializeField] TextMeshProUGUI puzzleName;

    public void UpdateDisplay(Picross puzzle)
    {
        UpdateCompletedImage(puzzle);

        for (int i = 0; i < puzzleIcons.Length; i++)
        {
            puzzleIcons[i].enabled = i == (int)puzzle.completionStatus;
        }

        if (puzzle.completionStatus == CompletionStatus.Complete)
        {
            puzzleName.text = puzzle.name;
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

        puzzleIcons[(int)CompletionStatus.Complete].sprite = spr;
    }
}