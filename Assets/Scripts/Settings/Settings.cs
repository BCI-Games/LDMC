using System;
using UnityEngine;

public static partial class Settings
{
    public static event Action Modified;
    public static void AddAndInvokeModificationCallback(Action callback)
    {
        callback();
        Modified += callback;
    }

    public static bool HasValue(string name) => typeof(Settings).HasStaticField(name);
    public static bool TryGetValue<T>(string name, out T output) where T : ValueProxy
    => typeof(Settings).TryGetStaticFieldValue(name, out output);

    public static bool HasProperty(string name) => typeof(Settings).HasStaticProperty(name);
    public static bool TryGetPropertyValue<T>(string name, out T output)
    => typeof(Settings).TryGetStaticPropertyValue(name, out output);


    #region Timing
    public static FloatProxy RestingStateDuration = new(180);

    [Space]
    public static FloatProxy OffBlockDuration = new(20);
    public static FloatProxy OnBlockDuration = new(12);

    [Space]
    public static FloatProxy CharacterActivePeriod = new(1);
    public static FloatProxy CharacterIdlePeriod = new(0.5f);
    public static float AnimationCycleDuration => CharacterActivePeriod + CharacterIdlePeriod;
    public static int OnBlockCycleCount => (int)(OnBlockDuration / AnimationCycleDuration);


    [Space]
    public static FloatProxy EpochLength = new(1.5f);
    public static FloatProxy InputPollingPeriod = new(1.5f);
    public static float MinimumBlockDuration => Mathf.Min(OffBlockDuration, OnBlockDuration);
    public static int MinimumSharedEpochCount => (int)(MinimumBlockDuration / EpochLength);
    #endregion


    #region Audio
    [Space]
    public static FloatProxy MasterVolume = new(0.5f);
    public static FloatProxy MusicVolume = new(0.5f);
    public static FloatProxy SfxVolume = new(0.5f);
    public static IntegerProxy MusicTrackIndex = new(0);
    #endregion


    #region Presentation
    [Space]
    public static BooleanProxy AnimationSimplified = new(false);
    public static ExclusiveBooleanProxy MonsterAnimationEnabled = new(true, AnimationSimplified);
    public static ExclusiveBooleanProxy SpriteDeformationEnabled = new(true, AnimationSimplified);
    public static BooleanProxy MonsterDisplayedInOffBlock = new(false);

    [Space]
    public static BooleanProxy CaptureSequenceEnabled = new(true);
    public static FloatProxy CaptureSequenceDuration = new(2);

    public static BooleanProxy WakeupSequenceEnabled = new(true);
    public static FloatProxy WakeupSequenceDuration = new(1);
    #endregion
}