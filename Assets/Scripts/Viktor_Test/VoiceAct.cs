using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class VoiceAct : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMPro.TextMeshProUGUI textBox;
    [SerializeField] private Image imageBox;
    [SerializeField] private Button nextButton;

    [Header("Content")]
    [SerializeField] List<string> dialogue;
    [SerializeField] List<Sprite> images;
    [SerializeField] List<AudioClip> voiceClips; // New list for audio files

    [Header("Audio Settings")]
    [SerializeField] private AudioSource voiceSource; // Reference to the AudioSource

    [Header("Events")]
    [SerializeField] private UnityEvent onStartDialogue;
    [SerializeField] private UnityEvent onEndDialogue;

    private int currentIndex = -1;

    public void NextDialogue()
    {
        if (currentIndex < 0)
        {
            onStartDialogue?.Invoke();
        }
        
        // Stop the current voice clip if one is playing
        if (voiceSource != null && voiceSource.isPlaying)
        {
            voiceSource.Stop();
        }

        currentIndex++;

        if (currentIndex < dialogue.Count)
        {
            // Update Text and Image
            textBox.text = dialogue[currentIndex];
            imageBox.sprite = images[currentIndex];

            // Play the corresponding voice clip
            if (voiceSource != null && currentIndex < voiceClips.Count && voiceClips[currentIndex] != null)
            {
                voiceSource.clip = voiceClips[currentIndex];
                voiceSource.Play();
            }
        }
        else
        {
            textBox.text = "";
            onEndDialogue?.Invoke();
            currentIndex = -1;
            // Note: Keep nextButton listener management in your input manager or button click
        }
    }
}