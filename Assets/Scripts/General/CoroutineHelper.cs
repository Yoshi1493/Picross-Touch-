using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper
{
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame { get; }

    static Dictionary<float, WaitForSeconds> _waitForSeconds = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_waitForSeconds.ContainsKey(seconds))
        {
            _waitForSeconds.Add(seconds, new WaitForSeconds(seconds));
        }
        return _waitForSeconds[seconds];
    }
}