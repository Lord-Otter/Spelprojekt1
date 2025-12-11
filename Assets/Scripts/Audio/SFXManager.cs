using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource SFXObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    /*public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        // Assign audioClip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get Length of SFX clip
        float clipLength = audioSource.clip.length;

        // Destroy the clip when finished
        Destroy(audioSource.gameObject, clipLength);
    }*/

    public AudioSource PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);

        return audioSource;
    }

    public void PlayRandomSFXClip(List<AudioClip> audioClips, Transform spawnTransform, float volume)
    {
        // Assign a random index
        int random = Random.Range(0, audioClips.Count);

        // Spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        // Assign audioClip
        audioSource.clip = audioClips[random];

        // assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get Length of SFX clip
        float clipLength = audioSource.clip.length;

        // Destroy the clip when finished
        Destroy(audioSource.gameObject, clipLength);
    }

    public void CancelSFXClip()
    {
        
    }
}
