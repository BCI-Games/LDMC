using UnityEngine;
using System.IO;

public static class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.persistentDataPath + "/" + FileName;

    private static SettingsContainer Container => _container??= LoadContainer();
    private static SettingsContainer _container;


    public static float MasterVolume {
        get => Container.MasterVolume;
        set { Container.MasterVolume = value; SaveContainer(); }
    }
    public static float MusicVolume {
        get => Container.MusicVolume;
        set { Container.MusicVolume = value; SaveContainer(); }
    }
    public static float SfxVolume {
        get => Container.SfxVolume;
        set { Container.SfxVolume = value; SaveContainer(); }
    }
    
    public static float CharacterReadyDuration {
        get => Container.CharacterAnimationTiming.Ready;
        set { Container.CharacterAnimationTiming.Ready = value; SaveContainer(); }
    }
    public static float CharacterActiveDuration {
        get => Container.CharacterAnimationTiming.Active;
        set { Container.CharacterAnimationTiming.Active = value; SaveContainer(); }
    }
    public static float CharacterReleaseDuration {
        get => Container.CharacterAnimationTiming.Release;
        set { Container.CharacterAnimationTiming.Release = value; SaveContainer(); }
    }
    public static float CharacterIdleDuration {
        get => Container.CharacterAnimationTiming.Idle;
        set { Container.CharacterAnimationTiming.Idle = value; SaveContainer(); }
    }

    public static bool SpriteAnimationEnabled {
        get => Container.SpriteAnimationEnabled;
        set { Container.SpriteAnimationEnabled = value; SaveContainer(); }
    }
    public static bool MeshAnimationEnabled {
        get => SpriteAnimationEnabled && Container.MeshAnimationEnabled;
        set { Container.MeshAnimationEnabled = value; SaveContainer(); }
    }

    public static SphereAnimationType SphereAnimation {
        get => Container.SphereAnimation;
        set { Container.SphereAnimation = value; SaveContainer(); }
    }

    public static bool ReadySequenceEnabled {
        get => Container.ReadySequenceEnabled && ReadySequenceDuration > 0;
        set { Container.ReadySequenceEnabled = value; SaveContainer(); }
    }
    public static float ReadySequenceDuration {
        get => Container.ReadySequenceDuration;
        set { Container.ReadySequenceDuration = value; SaveContainer(); }
    }

    public static bool CaptureSequenceEnabled {
        get => Container.CaptureSequenceEnabled && CaptureSequenceDuration > 0;
        set { Container.CaptureSequenceEnabled = value; SaveContainer(); }
    }
    public static float CaptureSequenceDuration {
        get => Container.CaptureSequenceDuration;
        set { Container.CaptureSequenceDuration = value; SaveContainer(); }
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

    private static void SaveContainer() => SaveContainer(Container);
    private static void SaveContainer(SettingsContainer container)
    {
        string fileContent = JsonUtility.ToJson(container, true);
        StreamWriter writer = new(FilePath);
        writer.Write(fileContent);
        writer.Close();
    }
}