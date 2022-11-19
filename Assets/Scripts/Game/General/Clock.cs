using System.Collections;
using UnityEngine;
using static CoroutineHelper;

public class Clock : MonoBehaviour
{
    IEnumerator clock;
    public float CurrentTime { get; private set; }

    public bool IsPaused { get; private set; }

    IEnumerator CountUp()
    {
        while (true)
        {
            yield return EndOfFrame;

            if (!IsPaused)
            {
                CurrentTime += Time.deltaTime;
            }
        }
    }

    public void StartClock(float startTime)
    {
        CurrentTime = startTime;

        clock = CountUp();
        StartCoroutine(clock);
    }

    public void StopClock()
    {
        StopCoroutine(clock);
    }

    public void SetPaused(bool state)
    {
        IsPaused = state;
    }

    public void RestartClock()
    {
        StopClock();
        StartClock(0f);
    }

    void Awake()
    {
        FindObjectOfType<PauseHandler>().GamePauseAction += SetPaused;
        GetComponent<Game>().GameOverAction += StopClock;
    }
}