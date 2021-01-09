using UnityEngine;
using UnityEngine.UI;
using static GameSettings;

public class SettingsMenu : Menu
{
    [SerializeField] Toggle autofillToggle, clockToggle;

    protected override void Awake()
    {
        base.Awake();
        InitUIObjects();
    }

    void InitUIObjects()
    {
        UpdateClockToggle();
        UpdateAutofillToggle();
    }

    void UpdateClockToggle()
    {
        clockToggle.SetIsOnWithoutNotify(playerSettings.clockEnabled);
    }

    void UpdateAutofillToggle()
    {
        autofillToggle.SetIsOnWithoutNotify(playerSettings.autofillEnabled);
    }

    public void OnToggleClock()
    {
        playerSettings.clockEnabled = !playerSettings.clockEnabled;
        UpdateClockToggle();
    }

    public void OnToggleAutofill()
    {
        playerSettings.autofillEnabled = !playerSettings.autofillEnabled;
        UpdateAutofillToggle();
    }

    public void SaveSettings()
    {
        FileHandler.SaveSettings();
    }
}