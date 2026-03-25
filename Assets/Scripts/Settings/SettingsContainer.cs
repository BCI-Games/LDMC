using System;

[Serializable]
public class SettingsContainer
{
    public float RestingStateDuration = Settings.RestingStateDuration;
    public float OffBlockDuration = Settings.OffBlockDuration;
    public int OnBlockCycleCount = Settings.OnBlockCycleCount;
    public bool EndOnBlockWithIdle = Settings.OnBlockEndedWithIdle;

    public float EpochLength = Settings.EpochLength;
    public float InputPollingPeriod = Settings.InputPollingPeriod;

    public CharacterAnimationTimings CharacterAnimationTiming = new();

    public float MasterVolume = Settings.MasterVolume;
    public float MusicVolume = Settings.MusicVolume;
    public float SfxVolume = Settings.SfxVolume;
    public int MusicTrackIndex = Settings.MusicTrackIndex;

    public bool SimplifyAnimation = Settings.AnimationSimplified;
    public bool EnableMonsterAnimation = Settings.MonsterAnimationEnabled.GetRawValue();
    public bool EnableSpriteDeformation = Settings.SpriteDeformationEnabled.GetRawValue();
    public bool EnableOffBlockMonsterDisplay = Settings.OffBockMonsterDisplayEnabled;

    public bool EnableWakeupSequence = Settings.WakeupSequenceEnabled;
    public float WakeupSequenceDuration = Settings.WakeupSequenceDuration;

    public bool EnableCaptureSequence = Settings.CaptureSequenceEnabled;
    public float CaptureSequenceDuration = Settings.CaptureSequenceDuration;


    [Serializable]
    public class CharacterAnimationTimings
    {
        public float Active = Settings.CharacterActiveDuration;
        public float Idle = Settings.CharacterIdleDuration;

        public void ApplyValues()
        {
            Settings.CharacterActiveDuration = new(Active);
            Settings.CharacterIdleDuration = new(Idle);
        }
    }


    public void ApplyValues()
    {
        Settings.RestingStateDuration = new(RestingStateDuration);
        Settings.OffBlockDuration = new(OffBlockDuration);
        Settings.OnBlockCycleCount = new(OnBlockCycleCount);
        Settings.OnBlockEndedWithIdle = new(EndOnBlockWithIdle);

        Settings.EpochLength = new(EpochLength);
        Settings.InputPollingPeriod = new(InputPollingPeriod);

        CharacterAnimationTiming.ApplyValues();

        Settings.MasterVolume = new(MasterVolume);
        Settings.MusicVolume = new(MusicVolume);
        Settings.SfxVolume = new(SfxVolume);
        Settings.MusicTrackIndex = new(MusicTrackIndex);

        BooleanProxy simpleAnimationProxy = new(SimplifyAnimation);
        Settings.AnimationSimplified = simpleAnimationProxy;
        Settings.MonsterAnimationEnabled = new(EnableMonsterAnimation, simpleAnimationProxy);
        Settings.SpriteDeformationEnabled = new(EnableSpriteDeformation, simpleAnimationProxy);
        Settings.OffBockMonsterDisplayEnabled = new(EnableOffBlockMonsterDisplay);

        Settings.WakeupSequenceEnabled = new(EnableWakeupSequence);
        Settings.WakeupSequenceDuration = new(WakeupSequenceDuration);

        Settings.CaptureSequenceEnabled = new(EnableCaptureSequence);
        Settings.CaptureSequenceDuration = new(CaptureSequenceDuration);
    }
}