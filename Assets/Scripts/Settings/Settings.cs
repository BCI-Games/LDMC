using UnityEngine;
using System.IO;
using System;

public static class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.persistentDataPath + "/" + FileName;

    private static SettingsContainer Container => _container??= LoadContainer();
    private static SettingsContainer _container;


    public static event Action Modified;
    public static void AddAndInvokeModificationCallback(Action callback)
    {
        callback();
        Modified += callback;
    }


    public static float OffBlockDuration {
        get => Container.OffBlockDuration;
        set { Container.OffBlockDuration = value; ApplyModifiedValue(); }
    }
    public static int OnBlockCycleCount {
        get => Container.OnBlockCycleCount;
        set { Container.OnBlockCycleCount = value; ApplyModifiedValue(); }
    }
    public static float AnimationCycleDuration => Container.CharacterAnimationTiming.TotalCycleTime;
    public static float OnBlockDuration => OnBlockCycleCount * AnimationCycleDuration;

    public static float MasterVolume {
        get => Container.MasterVolume;
        set { Container.MasterVolume = value; ApplyModifiedValue(); }
    }
    public static float MusicVolume {
        get => Container.MusicVolume;
        set { Container.MusicVolume = value; ApplyModifiedValue(); }
    }
    public static float SfxVolume {
        get => Container.SfxVolume;
        set { Container.SfxVolume = value; ApplyModifiedValue(); }
    }
    public static int MusicTrackIndex {
        get => Container.MusicTrackIndex;
        set { Container.MusicTrackIndex = value; ApplyModifiedValue(); }
    }
    
    // public static float CharacterReadyDuration {
    //     get => Container.CharacterAnimationTiming.Ready;
    //     set { Container.CharacterAnimationTiming.Ready = value; ApplyModifiedValue(); }
    // }
    public static float CharacterActiveDuration {
        get => Container.CharacterAnimationTiming.Active;
        set { Container.CharacterAnimationTiming.Active = value; ApplyModifiedValue(); }
    }
    // public static float CharacterReleaseDuration {
    //     get => Container.CharacterAnimationTiming.Release;
    //     set { Container.CharacterAnimationTiming.Release = value; ApplyModifiedValue(); }
    // }
    public static float CharacterIdleDuration {
        get => Container.CharacterAnimationTiming.Idle;
        set { Container.CharacterAnimationTiming.Idle = value; ApplyModifiedValue(); }
    }

    public static bool SpriteAnimationEnabled {
        get => Container.SpriteAnimationEnabled;
        set { Container.SpriteAnimationEnabled = value; ApplyModifiedValue(); }
    }
    public static bool MeshAnimationEnabled {
        get => SpriteAnimationEnabled && Container.MeshAnimationEnabled;
        set { Container.MeshAnimationEnabled = value; ApplyModifiedValue(); }
    }

    public static SphereAnimationType SphereAnimation {
        get => Container.SphereAnimation;
        set { Container.SphereAnimation = value; ApplyModifiedValue(); }
    }

    public static bool ReadySequenceEnabled {
        get => Container.WakeupSequenceEnabled && ReadySequenceDuration > 0;
        set { Container.WakeupSequenceEnabled = value; ApplyModifiedValue(); }
    }
    public static float ReadySequenceDuration {
        get => Container.WakeupSequenceDuration;
        set { Container.WakeupSequenceDuration = value; ApplyModifiedValue(); }
    }

    public static bool CaptureSequenceEnabled {
        get => Container.CaptureSequenceEnabled && CaptureSequenceDuration > 0;
        set { Container.CaptureSequenceEnabled = value; ApplyModifiedValue(); }
    }
    public static float CaptureSequenceDuration {
        get => Container.CaptureSequenceDuration;
        set { Container.CaptureSequenceDuration = value; ApplyModifiedValue(); }
    }


    private static SettingsContainer LoadContainer()
    {
        SettingsContainer loadedSettings = new();
        if (File.Exists(FilePath))
        {
            StreamReader reader = new(FilePath);
            string fileContent = reader.ReadToEnd();
            reader.Close();

            JsonUtility.FromJsonOverwrite(fileContent, loadedSettings);
        }
        SaveContainer(loadedSettings);
        return loadedSettings;
    }

    private static void ApplyModifiedValue()
    {
        SaveContainer(Container);
        Modified?.Invoke();
    }

    private static void SaveContainer(SettingsContainer container)
    {
        string fileContent = JsonUtility.ToJson(container, true);
        StreamWriter writer = new(FilePath);
        writer.Write(fileContent);
        writer.Close();
    }
}