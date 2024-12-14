using System;

[Serializable]
public class SettingsContainer
{
    public float OffBlockDuration = 20;
    public int OnBlockCycleCount = 3;
    public bool EndOnBlockWithIdle = true;

    public float MasterVolume = 0.5f;
    public float MusicVolume = 0.5f;
    public float SfxVolume = 0.5f;
    public int MusicTrackIndex = 0;

    public CharacterAnimationTimings CharacterAnimationTiming = new();

    public bool SimplifyAnimation = false;
    public bool EnableMonsterAnimation = true;
    public bool EnableSpriteDeformation = true;

    public bool EnablePixelPerfectCamera = true;

    public bool EnableWakeupSequence = true;
    public float WakeupSequenceDuration = 1.0f;

    public bool EnableCaptureSequence = true;
    public float CaptureSequenceDuration = 2.0f;


    public static implicit operator bool(SettingsContainer container) => container != null;

    [Serializable]
    public class CharacterAnimationTimings
    {
        public float Active = 2.0f;
        public float Idle = 2.0f;

        public float TotalCycleTime => Active + Idle;
    }
}