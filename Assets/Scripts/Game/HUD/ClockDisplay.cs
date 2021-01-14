using System;
using UnityEngine;
using TMPro;
using static GameSettings;

public class ClockDisplay : MonoBehaviour
{
    [SerializeField] Clock clock;
    TextMeshProUGUI timeText;

    void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();

        FindObjectOfType<SettingsMenu>().ToggleClockAction += OnClockToggled;
        OnClockToggled();
    }

    void Update()
    {
        timeText.text = TimeSpan.FromSeconds(clock.CurrentTime).ToString(TimeTextFormat);
    }

    void OnClockToggled()
    {
        gameObject.SetActive(playerSettings.clockEnabled);
    }
}