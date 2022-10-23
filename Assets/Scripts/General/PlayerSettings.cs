[System.Serializable]
public class PlayerSettings
{
    public int selectedDiffculty;
    public int selectedPuzzle;

    public bool clockEnabled;
    public bool autofillEnabled;
    public bool soundEnabled;

    public (float, float)[] levelSelectScreenPositions;

    public PlayerSettings()
    {
        selectedDiffculty = 0;

        clockEnabled = true;
        autofillEnabled = true;
        soundEnabled = true;

        levelSelectScreenPositions = new (float, float)[GameSettings.DifficultyCount];
    }

    public void UpdateSettings(PlayerSettings ps)
    {
        selectedDiffculty = ps.selectedDiffculty;

        clockEnabled = ps.clockEnabled;
        autofillEnabled = ps.autofillEnabled;
        soundEnabled = ps.soundEnabled;
    }

    public void SaveLevelSelectScreenPositions((float, float)[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            levelSelectScreenPositions[i] = positions[i];
        }
    }
}