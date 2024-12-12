using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders: MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;


    private void Start()
    {
        _masterVolumeSlider.value = Settings.MasterVolume;
        _musicVolumeSlider.value = Settings.MusicVolume;
        _sfxVolumeSlider.value = Settings.SfxVolume;

        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    private void SetMasterVolume(float value) => Settings.MasterVolume = value;
    private void SetMusicVolume(float value) => Settings.MusicVolume = value;
    private void SetSfxVolume(float value) => Settings.SfxVolume = value;
}