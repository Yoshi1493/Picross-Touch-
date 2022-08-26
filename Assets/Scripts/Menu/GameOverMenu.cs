using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameSettings;
using static CoroutineHelper;
using static FileHandler;

public class GameOverMenu : Menu
{
    [SerializeField] Image completedImage;
    [SerializeField] TextMeshProUGUI completedImageName;
    [SerializeField] TextMeshProUGUI timeElapsedText;

    protected override void Awake()
    {
        base.Awake();
        FindObjectOfType<Game>().GameOverAction += OnGameOver;
    }

    void OnGameOver()
    {
        StartCoroutine(DelayedOpen());
    }

    IEnumerator DelayedOpen()
    {
        UpdateCompletedImage();
        UpdateTexts();

        yield return WaitForSeconds(1);
        Open();
    }

    //set texture of completedImage to corresponding image of loaded puzzle
    void UpdateCompletedImage()
    {
        byte[] texData = LoadCompletedImage(targetPuzzleData);
        Texture2D tex2D = new(targetPuzzleData.ColCount, targetPuzzleData.RowCount)
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

        completedImage.sprite = spr;
    }

    void UpdateTexts()
    {
        completedImageName.text = targetPuzzleData.name;
        timeElapsedText.enabled = playerSettings.clockEnabled;
    }
}