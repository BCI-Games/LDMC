using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static partial class Settings
{
    public static event Action Modified;
    public static void AddAndInvokeModificationCallback(Action callback)
    {
        callback();
        Modified += callback;
    }

    public static bool GetField<T>(string name, out T output)
    where T : class
    {
        FieldInfo[] fields = typeof(Settings).GetFields();
        FieldInfo match = fields.FirstOrDefault(field => field.Name == name);
        output = match?.GetValue(null) as T;
        return output != null;
    }


    #region Timing
    public static FloatProxy RestingStateDuration = new(180);

    public static FloatProxy OffBlockDuration = new(20);
    public static IntProxy OnBlockCycleCount = new(3);
    public static BooleanProxy OnBlockEndedWithIdle = new(true);
    public static float AnimationCycleDuration => CharacterActiveDuration + CharacterIdleDuration;
    public static float OnBlockDuration => OnBlockCycleCount * AnimationCycleDuration
        - (OnBlockEndedWithIdle ? 0 : CharacterIdleDuration);


    public static FloatProxy EpochLength = new(1.5f);
    public static FloatProxy InputPollingPeriod = new(0.5f);
    public static int OffBlockEpochCount => (int)(OffBlockDuration / EpochLength);
    public static int OnBlockEpochCount => OnBlockCycleCount
        * (int)(CharacterActiveDuration / EpochLength);
    public static int MinimumSharedEpochCount => Mathf.Min(OffBlockEpochCount, OnBlockEpochCount);


    public static FloatProxy CharacterActiveDuration = new(2);
    public static FloatProxy CharacterIdleDuration = new(2);
    #endregion


    #region Audio
    public static FloatProxy MasterVolume = new(0.5f);
    public static FloatProxy MusicVolume = new(0.5f);
    public static FloatProxy SfxVolume = new(0.5f);
    public static IntProxy MusicTrackIndex = new(0);
    #endregion


    #region Presentation
    public static BooleanProxy AnimationSimplified = new(false);
    public static ExclusiveBooleanProxy MonsterAnimationEnabled = new(true, AnimationSimplified);
    public static ExclusiveBooleanProxy SpriteDeformationEnabled = new(true, AnimationSimplified);
    public static BooleanProxy OffBockMonsterDisplayEnabled = new(false);

    public static BooleanProxy WakeupSequenceEnabled = new(true);
    public static FloatProxy WakeupSequenceDuration = new(1);

    public static BooleanProxy CaptureSequenceEnabled = new(true);
    public static FloatProxy CaptureSequenceDuration = new(2);
    #endregion
}