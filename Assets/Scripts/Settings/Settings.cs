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


    public static float MasterVolume {
        get => Container.MasterVolume;
        set { Container.MasterVolume = value; OnValueModified(); }
    }
    public static float MusicVolume {
        get => Container.MusicVolume;
        set { Container.MusicVolume = value; OnValueModified(); }
    }
    public static float SfxVolume {
        get => Container.SfxVolume;
        set { Container.SfxVolume = value; OnValueModified(); }
    }
    
    public static float CharacterReadyDuration {
        get => Container.CharacterAnimationTiming.Ready;
        set { Container.CharacterAnimationTiming.Ready = value; OnValueModified(); }
    }
    public static float CharacterActiveDuration {
        get => Container.CharacterAnimationTiming.Active;
        set { Container.CharacterAnimationTiming.Active = value; OnValueModified(); }
    }
    public static float CharacterReleaseDuration {
        get => Container.CharacterAnimationTiming.Release;
        set { Container.CharacterAnimationTiming.Release = value; OnValueModified(); }
    }
    public static float CharacterIdleDuration {
        get => Container.CharacterAnimationTiming.Idle;
        set { Container.CharacterAnimationTiming.Idle = value; OnValueModified(); }
    }

    public static bool SpriteAnimationEnabled {
        get => Container.SpriteAnimationEnabled;
        set { Container.SpriteAnimationEnabled = value; OnValueModified(); }
    }
    public static bool MeshAnimationEnabled {
        get => SpriteAnimationEnabled && Container.MeshAnimationEnabled;
        set { Container.MeshAnimationEnabled = value; OnValueModified(); }
    }

    public static SphereAnimationType SphereAnimation {
        get => Container.SphereAnimation;
        set { Container.SphereAnimation = value; OnValueModified(); }
    }

    public static bool ReadySequenceEnabled {
        get => Container.ReadySequenceEnabled && ReadySequenceDuration > 0;
        set { Container.ReadySequenceEnabled = value; OnValueModified(); }
    }
    public static float ReadySequenceDuration {
        get => Container.ReadySequenceDuration;
        set { Container.ReadySequenceDuration = value; OnValueModified(); }
    }

    public static bool CaptureSequenceEnabled {
        get => Container.CaptureSequenceEnabled && CaptureSequenceDuration > 0;
        set { Container.CaptureSequenceEnabled = value; OnValueModified(); }
    }
    public static float CaptureSequenceDuration {
        get => Container.CaptureSequenceDuration;
        set { Container.CaptureSequenceDuration = value; OnValueModified(); }
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
            return loadedSettings;
        }
        SaveContainer(loadedSettings);
        return loadedSettings;
    }

    private static void OnValueModified()
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