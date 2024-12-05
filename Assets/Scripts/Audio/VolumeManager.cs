using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager: MonoBehaviour
{
    [SerializeField] private AudioMixer _masterMixer;
    

    private void Start()
    {
        ApplyVolumes();
    }


    private void ApplyVolumes()
    {
        SetMixerGroupVolume("Master", Settings.MasterVolume);
        SetMixerGroupVolume("Music", Settings.MusicVolume);
        SetMixerGroupVolume("Sfx", Settings.SfxVolume);
    }

    private void SetMixerGroupVolume(string groupName, float linearVolume)
    {
        float clampedVolume = Mathf.Clamp(linearVolume, 0.0001f, 1);
        float decibelVolume = Mathf.Log10(clampedVolume) * 20;
        _masterMixer.SetFloat(groupName + " Volume", decibelVolume);
    }
}