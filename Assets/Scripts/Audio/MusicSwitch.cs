using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    public static MusicSwitch instance;

    [Header("Audio Sources")]
    public AudioSource introSource; // Assign the first AudioSource here
    public AudioSource loopSource;  // Assign the second AudioSource here

    [Header("Audio Clips")]
    public AudioClip introClip;
    public AudioClip loopClip;

    void Awake()
    {
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
        if (instance == this && !introSource.isPlaying && !loopSource.isPlaying) 
        {
            PlayGaplessMusic();
        }
    }

    void PlayGaplessMusic()
    {
        if (introClip == null || loopClip == null)
        {
            Debug.LogError("MusicSwitch is missing clips!");
            return;
        }

        // 1. Get the current precise time of the audio engine
        double startTime = AudioSettings.dspTime + 0.2; // 0.2s buffer to ensure sync
        
        // 2. Calculate exactly when the intro will end
        double introDuration = (double)introClip.samples / introClip.frequency;
        double loopStartTime = startTime + introDuration;

        // 3. Setup Intro Source
        introSource.clip = introClip;
        introSource.loop = false;
        introSource.PlayScheduled(startTime);

        // 4. Setup Loop Source (It will wait silently until the exact time)
        loopSource.clip = loopClip;
        loopSource.loop = true;
        loopSource.PlayScheduled(loopStartTime);

        Debug.Log("Intro scheduled for: " + startTime);
        Debug.Log("Loop scheduled for: " + loopStartTime);
    }
}