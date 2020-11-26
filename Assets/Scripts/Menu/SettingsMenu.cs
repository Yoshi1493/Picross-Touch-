using UnityEngine;
using UnityEngine.UI;
using static GameSettings;

public class SettingsMenu : Menu
{
    [SerializeField] Toggle autofillToggle, sfxToggle;

    protected override void Awake()
    {
        base.Awake();
        InitUIObjects();
    }

    void InitUIObjects()
    {
        UpdateSFXToggle();
        UpdateAutofillToggle();
    }

    void UpdateSFXToggle()
    {
        sfxToggle.SetIsOnWithoutNotify(playerSettings.sfxEnabled);
    }

    void UpdateAutofillToggle()
    {
        autofillToggle.SetIsOnWithoutNotify(playerSettings.autofillEnabled);
    }

    public void OnToggleSFX()
    {
        playerSettings.sfxEnabled = !playerSettings.sfxEnabled;
        UpdateSFXToggle();
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