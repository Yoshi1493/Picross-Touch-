using System.Collections.Generic;
using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    [SerializeField] AudioClip cellFillSfx;
    [SerializeField] AudioClip gameoverSfx;

    void Awake()
    {
        GetComponent<Game>().GameOverAction += OnGameOver;
        GetComponent<InputHandler>().HighlightEndAction += OnHighlightEnd;
    }

    void OnHighlightEnd(List<Vector2Int> selectedCells)
    {
        AudioController.Instance.PlaySound(cellFillSfx);
    }

    void OnGameOver()
    {
        AudioController.Instance.PlaySound(gameoverSfx);
    }
}