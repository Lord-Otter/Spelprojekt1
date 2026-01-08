using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    public static MusicSwitch instance; // Singleton reference

    [Header("Music Sources")]
    public AudioSource introSource;
    public AudioSource loopSource;

    void Awake()
    {
        // Check if a music manager already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this alive!
        }
        else
        {
            Destroy(gameObject); // Destroy the new one if one already exists
            return;
        }
    }

    void Start()
    {
        // Ensure this only runs for the original instance
        if(instance == this)
        {
            introSource.Play();
            Invoke(nameof(PlayLoop), introSource.clip.length);
        }
    }

    void PlayLoop()
    {
        loopSource.Play();
    }
}