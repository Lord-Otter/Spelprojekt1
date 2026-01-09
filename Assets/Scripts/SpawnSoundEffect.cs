using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpawnSoundEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioClip spawnClip;
    [SerializeField] private bool playOnAwake = true;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Ensure the AudioSource is set up correctly
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // 3D sound
    }

    void Start()
    {
        if (playOnAwake && spawnClip != null)
        {
            PlaySpawnSound();
        }
    }

    public void PlaySpawnSound()
    {
        audioSource.PlayOneShot(spawnClip);
    }
}