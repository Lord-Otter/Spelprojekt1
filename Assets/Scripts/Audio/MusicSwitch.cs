using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    public static MusicSwitch instance;

    [Header("Setup")]
    public AudioSource introSource;
    public AudioSource loopSource;
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
        }
    }

    void Start()
    {
        PlayMusic();
    }

    void Update()
    {
        // Check for T key press
        if (Input.GetKeyDown(KeyCode.T))
        {
            SkipToLoopTransition();
        }
    }

    void PlayMusic()
    {
        introSource.clip = introClip;
        loopSource.clip = loopClip;
        loopSource.loop = true;

        double startTime = AudioSettings.dspTime;

        introSource.PlayScheduled(startTime);

        double introDuration = (double)introClip.samples / introClip.frequency;
        loopSource.PlayScheduled(startTime + introDuration);
    }

    void SkipToLoopTransition()
    {
        if (!introSource.isPlaying)
        {
            Debug.Log("Intro is not playing!");
            return;
        }

        // Get intro duration
        float introDuration = introClip.length;
        
        // Get current time in intro
        float currentTime = introSource.time;
        
        // Calculate time to skip to (5 seconds before end)
        float skipToTime = introDuration - 5.0f;
        
        // Don't skip if we're already at or past that point
        if (currentTime >= skipToTime)
        {
            Debug.Log($"Already at or past skip point. Current: {currentTime:F2}s, Skip to: {skipToTime:F2}s");
            return;
        }
        
        Debug.Log($"Skipping from {currentTime:F2}s to {skipToTime:F2}s (5 seconds before end)");
        
        // Stop current playback
        introSource.Stop();
        loopSource.Stop();
        
        // Get current DSP time for scheduling
        double dspTime = AudioSettings.dspTime + 0.1; // Small buffer
        
        // Play intro from skip point
        introSource.time = skipToTime;
        introSource.PlayScheduled(dspTime);
        
        // Schedule loop to start 5 seconds later
        loopSource.PlayScheduled(dspTime + 5.0);
    }
}