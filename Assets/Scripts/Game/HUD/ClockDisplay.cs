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
        gameObject.SetActive(playerSettings.clockEnabled);
    }

    void Update()
    {
        timeText.text = TimeSpan.FromSeconds(clock.CurrentTime).ToString(TimeTextFormat);
    }
}