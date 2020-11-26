public class PauseMenu : Menu
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<PauseHandler>().GamePauseAction += OnPausedStateChanged;
        FindObjectOfType<Game>().GameOverAction += OnGameOver;
    }
    
    void OnPausedStateChanged(bool state)
    {
        if (state) Open();
        else Close();
    }

    void OnGameOver()
    {
        GetComponent<PauseHandler>().GamePauseAction -= OnPausedStateChanged;
    }
}