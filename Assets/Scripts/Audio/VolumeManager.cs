using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager: MonoBehaviour
{
    const string MixerPath = "Audio/Global Audio";
    public static AudioMixer GlobalAudioMixer
        => _globalAudioMixer ??= GetGlobalAudioMixer();
    private static  AudioMixer _globalAudioMixer;
    

    private void Start() => Settings.AddAndInvokeModificationCallback(ApplyVolumes);
    private void OnDestroy() => Settings.Modified -= ApplyVolumes;


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
        GlobalAudioMixer.SetFloat(groupName + " Volume", decibelVolume);
    }

    public static AudioMixer GetGlobalAudioMixer()
        => Resources.Load<AudioMixer>(MixerPath);
}