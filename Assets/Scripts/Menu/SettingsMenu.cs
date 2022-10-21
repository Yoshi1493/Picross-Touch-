using System;
using UnityEngine;
using UnityEngine.UI;
using static GameSettings;

public class SettingsMenu : Menu
{
    [SerializeField] Toggle autofillToggle, clockToggle, soundToggle;

    public event Action ToggleClockAction;

    protected override void Awake()
    {
        base.Awake();
        InitUIObjects();

        ToggleClockAction += UpdateClockToggle;
    }

    void InitUIObjects()
    {
        UpdateAutofillToggle();
        UpdateClockToggle();
        UpdateSoundToggle();
    }

    void UpdateAutofillToggle()
    {
        autofillToggle.SetIsOnWithoutNotify(playerSettings.autofillEnabled);
    }

    void UpdateClockToggle()
    {
        clockToggle.SetIsOnWithoutNotify(playerSettings.clockEnabled);
    }

    void UpdateSoundToggle()
    {
        soundToggle.SetIsOnWithoutNotify(playerSettings.soundEnabled);
    }

    #region Button functions

    public void OnToggleAutofill()
    {
        playerSettings.autofillEnabled = !playerSettings.autofillEnabled;
        UpdateAutofillToggle();
    }

    public void OnToggleClock()
    {
        playerSettings.clockEnabled = !playerSettings.clockEnabled;
        ToggleClockAction?.Invoke();
    }

    public void OnToggleSound()
    {
        playerSettings.soundEnabled = !playerSettings.soundEnabled;
        UpdateSoundToggle();
    }

    #endregion

    public void SaveSettings()
    {
        FileHandler.SaveSettings();
    }
}