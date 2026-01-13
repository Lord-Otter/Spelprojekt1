using UnityEngine;
using UnityEngine.UI;

public class VolumeSlidersUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        // Set slider values FIRST (no callbacks yet)
        masterSlider.SetValueWithoutNotify(
            SoundMixerManager.Instance.GetMasterVolume()
        );

        sfxSlider.SetValueWithoutNotify(
            SoundMixerManager.Instance.GetSFXVolume()
        );

        musicSlider.SetValueWithoutNotify(
            SoundMixerManager.Instance.GetMusicVolume()
        );
    }

    public void OnMasterChanged(float value)
    {
        SoundMixerManager.Instance.SetMasterVolume(value);
    }

    public void OnSFXChanged(float value)
    {
        SoundMixerManager.Instance.SetSFXVolume(value);
    }

    public void OnMusicChanged(float value)
    {
        SoundMixerManager.Instance.SetMusicVolume(value);
    }
}