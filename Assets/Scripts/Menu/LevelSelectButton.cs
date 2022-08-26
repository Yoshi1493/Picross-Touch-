using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static FileHandler;
using static GameSettings;

public class LevelSelectButton : MonoBehaviour
{
    public TextAsset data;

    [SerializeField] Image buttonFillImage;
    [SerializeField] Image[] puzzleIcons = new Image[System.Enum.GetNames(typeof(CompletionStatus)).Length];
    [SerializeField] TextMeshProUGUI puzzleName;

    public void UpdateDisplay(Picross puzzle)
    {
        UpdateCompletedImage(puzzle);

        //enable completion icon based on puzzle's completion status
        for (int i = 0; i < puzzleIcons.Length; i++)
        {
            puzzleIcons[i].enabled = i == (int)puzzle.completionStatus;
        }

        //if puzzle is complete, also display puzzle name
        if (puzzle.completionStatus == CompletionStatus.Complete)
        {
            puzzleName.text = puzzle.name;
        }
    }
    
    void UpdateCompletedImage(Picross puzzle)
    {
        byte[] texData = LoadCompletedImage(puzzle);
        if (texData == null) return;

        //create new texture with dimensions equal to puzzle's row/column count
        Texture2D tex2D = new (puzzle.ColCount, puzzle.RowCount)
        {
            filterMode = FilterMode.Point
        };

        tex2D.LoadImage(texData);

        //create new sprite based on texture data
        Sprite spr = Sprite.Create(
            tex2D,
            new Rect(0, 0, tex2D.width, tex2D.height),
            Vector2.zero / 2f,
            Mathf.Min(tex2D.width, tex2D.height)
            );

        //set puzzle's "completed" icon to created sprite
        puzzleIcons[(int)CompletionStatus.Complete].sprite = spr;
        buttonFillImage.color = Color.white;
    }
}