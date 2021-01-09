[System.Serializable]
public class PlayerSettings
{
    public int selectedDiffculty;
    public int selectedPuzzle;

    public bool clockEnabled;
    public bool autofillEnabled;

    public PlayerSettings()
    {
        selectedDiffculty = 0;

        clockEnabled = true;
        autofillEnabled = true;
    }

    public void UpdateSettings(PlayerSettings ps)
    {
        selectedDiffculty = ps.selectedDiffculty;

        clockEnabled = ps.clockEnabled;
        autofillEnabled = ps.autofillEnabled;
    }
}