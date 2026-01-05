using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    [Header("Music Sources")]
    public AudioSource introSource;
    public AudioSource loopSource;

    void Start()
    {
        introSource.Play();
        Invoke(nameof(PlayLoop), introSource.clip.length);
    }

    void PlayLoop()
    {
        loopSource.Play();
    }
}
