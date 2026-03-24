using UnityEngine;
using System;

public static partial class Settings
{
    private static SettingsContainer Container => _container??= LoadContainer();
    private static SettingsContainer _container;
    private static GameObject _audioManager;


    public static event Action Modified;
    public static void AddAndInvokeModificationCallback(Action callback)
    {
        callback();
        Modified += callback;
    }

    private static void ApplyModifiedValue()
    {
        SaveContainer(Container);
        Modified?.Invoke();
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
    public static bool OffBockMonsterDisplayEnabled {
        get => Container.EnableOffBlockMonsterDisplay;
        set { Container.EnableOffBlockMonsterDisplay = value; ApplyModifiedValue(); }
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
}