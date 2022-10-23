using UnityEngine;
using static GameSettings;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    AudioSource aux;

    static readonly AudioController instance;
    public static AudioController Instance => instance != null ? instance : FindObjectOfType<AudioController>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        aux = Instance.GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && playerSettings.soundEnabled)
        {
            aux.PlayOneShot(clip);
        }
    }
}