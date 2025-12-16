using UnityEngine;
using UnityEngine.UI;

public class UIAudioSettings : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    public void Bind(SoundMixerManager mixer)
    {
        masterSlider.onValueChanged.AddListener(mixer.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(mixer.SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(mixer.SetSFXVolume);
    }
}
