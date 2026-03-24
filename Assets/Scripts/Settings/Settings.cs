using System;
using UnityEngine;

public static partial class Settings
{
    private static SettingsContainer Container => _container ??= LoadContainer();
    private static SettingsContainer _container;
    private static GameObject _audioManager;


    public static event Action Modified;
    public static void AddAndInvokeModificationCallback(Action callback)
    {
        callback();
        Modified += callback;
    }

    private static void UpdateValue<T>(ref T target, T value)
    {
        if (target.Equals(value)) return;

        target = value;
        SaveContainer(Container);
        Modified?.Invoke();
    }


    #region Timing
    public static float OffBlockDuration
    {
        get => Container.OffBlockDuration;
        set => UpdateValue(ref Container.OffBlockDuration, value);
    }
    public static int OnBlockCycleCount
    {
        get => Container.OnBlockCycleCount;
        set => UpdateValue(ref Container.OnBlockCycleCount, value);
    }
    public static bool OnBlockEndedWithIdle
    {
        get => Container.EndOnBlockWithIdle;
        set => UpdateValue(ref Container.EndOnBlockWithIdle, value);
    }
    public static float AnimationCycleDuration => Container.CharacterAnimationTiming.TotalCycleTime;
    public static float OnBlockDuration => OnBlockCycleCount * AnimationCycleDuration
        - (OnBlockEndedWithIdle ? 0 : Container.CharacterAnimationTiming.Idle);


    public static float EpochLength
    {
        get => Container.EpochLength;
        set => UpdateValue(ref Container.EpochLength, value);
    }
    public static float InputPollingPeriod
    {
        get => Container.InputPollingPeriod;
        set => UpdateValue(ref Container.InputPollingPeriod, value);
    }
    public static int OffBlockEpochCount => (int)(OffBlockDuration / EpochLength);
    public static int OnBlockEpochCount => OnBlockCycleCount
        * (int)(CharacterActiveDuration / EpochLength);


    public static float CharacterActiveDuration
    {
        get => Container.CharacterAnimationTiming.Active;
        set => UpdateValue(ref Container.CharacterAnimationTiming.Active, value);
    }
    public static float CharacterIdleDuration
    {
        get => Container.CharacterAnimationTiming.Idle;
        set => UpdateValue(ref Container.CharacterAnimationTiming.Idle, value);
    }
    #endregion


    #region Audio
    public static float MasterVolume
    {
        get => Container.MasterVolume;
        set => UpdateValue(ref Container.MasterVolume, value);
    }
    public static float MusicVolume
    {
        get => Container.MusicVolume;
        set => UpdateValue(ref Container.MusicVolume, value);
    }
    public static float SfxVolume
    {
        get => Container.SfxVolume;
        set => UpdateValue(ref Container.SfxVolume, value);
    }
    public static int MusicTrackIndex
    {
        get => Container.MusicTrackIndex;
        set => UpdateValue(ref Container.MusicTrackIndex, value);
    }
    #endregion


    #region Presentation
    public static bool AnimationSimplified
    {
        get => Container.SimplifyAnimation;
        set => UpdateValue(ref Container.SimplifyAnimation, value);
    }
    public static bool MonsterAnimationEnabled
    {
        get => Container.EnableMonsterAnimation && !AnimationSimplified;
        set => UpdateValue(ref Container.EnableMonsterAnimation, value);
    }
    public static bool SpriteDeformationEnabled
    {
        get => Container.EnableSpriteDeformation && !AnimationSimplified;
        set => UpdateValue(ref Container.EnableSpriteDeformation, value);
    }
    public static bool OffBockMonsterDisplayEnabled
    {
        get => Container.EnableOffBlockMonsterDisplay;
        set => UpdateValue(ref Container.EnableOffBlockMonsterDisplay, value);
    }

    #region Feedback Sequences
    public static bool WakeupSequenceEnabled
    {
        get => Container.EnableWakeupSequence && WakeupSequenceDuration > 0;
        set => UpdateValue(ref Container.EnableWakeupSequence, value);
    }
    public static float WakeupSequenceDuration
    {
        get => Container.WakeupSequenceDuration;
        set => UpdateValue(ref Container.WakeupSequenceDuration, value);
    }

    public static bool CaptureSequenceEnabled
    {
        get => Container.EnableCaptureSequence && CaptureSequenceDuration > 0;
        set => UpdateValue(ref Container.EnableCaptureSequence, value);
    }
    public static float CaptureSequenceDuration
    {
        get => Container.CaptureSequenceDuration;
        set => UpdateValue(ref Container.CaptureSequenceDuration, value);
    }
    #endregion
    #endregion
}