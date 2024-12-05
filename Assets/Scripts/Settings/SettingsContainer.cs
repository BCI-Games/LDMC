public enum SphereAnimationType {Physics, Interpolated, Discrete}

public class SettingsContainer
{
    public float Volume = 1.0f;

    public CharacterAnimationTimings CharacterAnimationTiming = new();

    public bool SpriteAnimationEnabled = true;
    public bool MeshAnimationEnabled = false;

    public SphereAnimationType SphereAnimation = SphereAnimationType.Physics;

    public bool ReadySequenceEnabled = true;
    public float ReadySequenceDuration = 1.0f;

    public bool CaptureSequenceEnabled = true;
    public float CaptureSequenceDuration = 2.0f;


    public static implicit operator bool(SettingsContainer container) => container != null;

    public class CharacterAnimationTimings
    {
        public float Ready = 0.25f;
        public float Active = 2.0f;
        public float Release = 0.25f;
        public float Idle = 1.5f;
    }
}