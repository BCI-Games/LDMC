using System;

public enum SphereAnimationType {Physics, Interpolated, Discrete}

[Serializable]
public class SettingsContainer
{
    public float OffBlockDuration = 20;
    public int OnBlockCycleCount = 3;

    public float MasterVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float SfxVolume = 1.0f;
    public int MusicTrackIndex = 0;

    public CharacterAnimationTimings CharacterAnimationTiming = new();

    public bool SimplifyAnimation = false;
    public bool EnableMonsterAnimation = true;
    public bool EnableMeshAnimation = true;

    public SphereAnimationType SphereAnimation = SphereAnimationType.Physics;

    public bool EnableWakeupSequence = true;
    public float WakeupSequenceDuration = 1.0f;

    public bool EnableCaptureSequence = true;
    public float CaptureSequenceDuration = 2.0f;


    public static implicit operator bool(SettingsContainer container) => container != null;

    [Serializable]
    public class CharacterAnimationTimings
    {
        public float Active = 2.0f;
        public float Idle = 1.5f;

        public float TotalCycleTime => Active + Idle;
    }
}