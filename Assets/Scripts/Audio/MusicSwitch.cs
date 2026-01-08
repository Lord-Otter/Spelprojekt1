using UnityEngine;
using System.Collections;

public class MusicSwitch : MonoBehaviour
{
    public static MusicSwitch instance;

    [Header("Audio Settings")]
    public AudioSource musicPlayer;
    public AudioClip introClip;
    public AudioClip loopClip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("<color=green>MusicManager created and marked as DontDestroyOnLoad.</color>");
        }
        else
        {
            Debug.Log("<color=yellow>Duplicate MusicManager detected in new scene. Destroying duplicate.</color>");
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        if (instance == this && !musicPlayer.isPlaying) 
        {
            if (introClip != null)
            {
                Debug.Log("Starting Intro Clip: " + introClip.name);
                musicPlayer.clip = introClip;
                musicPlayer.loop = false;
                musicPlayer.Play();
                
                // Start the timer to switch to loop
                StartCoroutine(WaitForLoop(introClip.length));
            }
            else if (loopClip != null)
            {
                Debug.Log("No Intro found, jumping straight to Loop.");
                PlayLoop();
            }
        }
    }

    IEnumerator WaitForLoop(float delay)
    {
        Debug.Log("Waiting " + delay + " seconds for intro to finish...");
        yield return new WaitForSecondsRealtime(delay);
        
        Debug.Log("<color=cyan>Intro finished! Switching to Loop now.</color>");
        PlayLoop();
    }

    void PlayLoop()
    {
        if (loopClip != null)
        {
            musicPlayer.clip = loopClip;
            musicPlayer.loop = true; 
            musicPlayer.Play();
            Debug.Log("Now playing Loop Clip: " + loopClip.name);
        }
        else
        {
            Debug.LogError("Loop Clip is missing in the Inspector!");
        }
    }
}