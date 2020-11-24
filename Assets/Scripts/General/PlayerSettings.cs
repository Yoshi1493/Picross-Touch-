[System.Serializable]
public class PlayerSettings
{
    public int selectedDiffculty;
    public int selectedPuzzle;

    public bool sfxEnabled;
    public bool autofillEnabled;

    public PlayerSettings()
    {
        selectedDiffculty = 0;

        sfxEnabled = true;
        autofillEnabled = true;
    }

    public void UpdateSettings(PlayerSettings ps)
    {
        selectedDiffculty = ps.selectedDiffculty;

        sfxEnabled = ps.sfxEnabled;
        autofillEnabled = ps.autofillEnabled;
    }
}