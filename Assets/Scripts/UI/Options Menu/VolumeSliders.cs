using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders: MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;


    private void Start()
    {
        Settings.MasterVolume.ConnectSlider(_masterVolumeSlider);
        Settings.MusicVolume.ConnectSlider(_musicVolumeSlider);
        Settings.SfxVolume.ConnectSlider(_sfxVolumeSlider);
    }
}