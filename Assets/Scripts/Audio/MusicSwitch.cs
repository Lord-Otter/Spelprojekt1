using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    public static MusicSwitch instance;

    [Header("Audio Settings")]
    public AudioSource musicPlayer; // One AudioSource is enough
    public AudioClip introClip;
    public AudioClip loopClip;

    void Awake()
    {
        // Singleton pattern to keep music playing across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (introClip != null)
        {
            musicPlayer.clip = introClip;
            musicPlayer.loop = false;
            musicPlayer.Play();
            
            // Schedule the loop to start exactly when the intro ends
            Invoke(nameof(PlayLoop), introClip.length);
        }
        else if (loopClip != null)
        {
            PlayLoop();
        }
    }

    void PlayLoop()
    {
        musicPlayer.clip = loopClip;
        musicPlayer.loop = true; // Make sure it stays looping
        musicPlayer.Play();
    }
}