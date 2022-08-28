using System;
using UnityEngine;
using UnityEngine.UI;
using static GameSettings;

public class SettingsMenu : Menu
{
    [SerializeField] Toggle autofillToggle, clockToggle;

    public event Action ToggleClockAction;

    protected override void Awake()
    {
        base.Awake();
        InitUIObjects();

        ToggleClockAction += UpdateClockToggle;
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
        ToggleClockAction?.Invoke();
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