using UnityEngine;
using System.IO;
using System;
using UnityEngine.Rendering;

public static class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.dataPath + "/../" + FileName;

    private static SettingsContainer Container => _container??= LoadContainer();
    private static SettingsContainer _container;
    private static GameObject _audioManager;


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
    public static bool OnBlockEndedWithIdle {
        get => Container.EndOnBlockWithIdle;
        set { Container.EndOnBlockWithIdle = value; ApplyModifiedValue(); }
    }
    public static float AnimationCycleDuration => Container.CharacterAnimationTiming.TotalCycleTime;
    public static float OnBlockDuration => OnBlockCycleCount * AnimationCycleDuration
        - (OnBlockEndedWithIdle? 0: Container.CharacterAnimationTiming.Idle);

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
    
    public static float CharacterActiveDuration {
        get => Container.CharacterAnimationTiming.Active;
        set { Container.CharacterAnimationTiming.Active = value; ApplyModifiedValue(); }
    }
    public static float CharacterIdleDuration {
        get => Container.CharacterAnimationTiming.Idle;
        set { Container.CharacterAnimationTiming.Idle = value; ApplyModifiedValue(); }
    }

    public static bool AnimationSimplified {
        get => Container.SimplifyAnimation;
        set { Container.SimplifyAnimation = value; ApplyModifiedValue(); }
    }
    public static bool MonsterAnimationEnabled {
        get => Container.EnableMonsterAnimation && !AnimationSimplified;
        set { Container.EnableMonsterAnimation = value; ApplyModifiedValue(); }
    }
    public static bool SpriteDeformationEnabled {
        get => Container.EnableSpriteDeformation && !AnimationSimplified;
        set { Container.EnableSpriteDeformation = value; ApplyModifiedValue(); }
    }

    public static SphereAnimationType SphereAnimation {
        get => Container.SphereAnimation;
        set { Container.SphereAnimation = value; ApplyModifiedValue(); }
    }

    public static bool WakeupSequenceEnabled {
        get => Container.EnableWakeupSequence && WakeupSequenceDuration > 0;
        set { Container.EnableWakeupSequence = value; ApplyModifiedValue(); }
    }
    public static float WakeupSequenceDuration {
        get => Container.WakeupSequenceDuration;
        set { Container.WakeupSequenceDuration = value; ApplyModifiedValue(); }
    }

    public static bool CaptureSequenceEnabled {
        get => Container.EnableCaptureSequence && CaptureSequenceDuration > 0;
        set { Container.EnableCaptureSequence = value; ApplyModifiedValue(); }
    }
    public static float CaptureSequenceDuration {
        get => Container.CaptureSequenceDuration;
        set { Container.CaptureSequenceDuration = value; ApplyModifiedValue(); }
    }


    public static void LoadAndApplySettings()
    {
        if (_container) return;
        _container = LoadContainer();
        ApplyModifiedValue();
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
        if (!_audioManager) InitializeAudioManager();
        return loadedSettings;
    }

    private static void InitializeAudioManager()
    {
        _audioManager = new("Audio Manager");
        _audioManager.AddComponent<VolumeManager>();
        _audioManager.AddComponent<MusicManager>();
        GameObject.DontDestroyOnLoad(_audioManager);
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