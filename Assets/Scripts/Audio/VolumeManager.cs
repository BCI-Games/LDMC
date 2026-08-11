using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager: MonoBehaviour
{
    const string MixerPath = "Audio/Global Audio";
    const string MasterGroupName = "Master";
    const string MusicGroupName = "Music";
    const string SfxGroupName = "Sfx";

    public static AudioMixer GlobalAudioMixer
        => _globalAudioMixer ??= GetGlobalAudioMixer();
    private static  AudioMixer _globalAudioMixer;
    

    private void Start() => Settings.AddAndInvokeModificationCallback(ApplyVolumes);
    private void OnDestroy() => Settings.Modified -= ApplyVolumes;


    public static void SetRelativeMusicVolume(float linearVolume)
    => SetMixerGroupVolume(MusicGroupName, Settings.MusicVolume * linearVolume);
    public static void ResetMusicVolume() => SetMixerGroupVolume(MusicGroupName, Settings.MusicVolume);
    public static void MuteMusic() => SetMixerGroupVolume(MusicGroupName, 0);

    public static void SetRelativeSfxVolume(float linearVolume)
    => SetMixerGroupVolume(SfxGroupName, Settings.MusicVolume * linearVolume);
    public static void ResetSfxVolume() => SetMixerGroupVolume(SfxGroupName, Settings.SfxVolume);
    public static void MuteSfx() => SetMixerGroupVolume(SfxGroupName, 0);


    private void ApplyVolumes()
    {
        SetMixerGroupVolume(MasterGroupName, Settings.MasterVolume);
        SetMixerGroupVolume(MusicGroupName, Settings.MusicVolume);
        SetMixerGroupVolume(SfxGroupName, Settings.SfxVolume);
    }

    private static void SetMixerGroupVolume(string groupName, float linearVolume)
    {
        float clampedVolume = Mathf.Clamp(linearVolume, 0.0001f, 1);
        float decibelVolume = Mathf.Log10(clampedVolume) * 20;
        GlobalAudioMixer.SetFloat(groupName + " Volume", decibelVolume);
    }

    public static AudioMixer GetGlobalAudioMixer()
        => Resources.Load<AudioMixer>(MixerPath);
}