using UnityEngine;

public class GameBoard : MonoBehaviour
{
    BoxCollider2D gameBoardCollider;

    void Awake()
    {
        gameBoardCollider = GetComponent<BoxCollider2D>();

        FindObjectOfType<Game>().GameOverAction += OnGameOver;
        FindObjectOfType<PauseHandler>().GamePauseAction += SetColliderState;
    }

    //disable collider to prevent board state changes when game is paused
    void SetColliderState(bool state)
    {
        gameBoardCollider.enabled = !state;
    }

    void OnGameOver()
    {
        SetColliderState(false);
    }
}