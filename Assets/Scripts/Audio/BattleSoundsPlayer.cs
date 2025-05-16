using UnityEngine;

public class BattleSoundsPlayer : MonoBehaviour
{
    [SerializeField] private RandomSoundEffectPlayer _throwSounds;
    [SerializeField] private RandomSoundEffectPlayer _restSounds;
    [SerializeField] private RandomSoundEffectPlayer _wakeupSounds;
    [SerializeField] private RandomSoundEffectPlayer _appearSounds;
    [SerializeField] private RandomSoundEffectPlayer _hitSounds;
    [SerializeField] private RandomSoundEffectPlayer _captureSounds;

    private AudioClip[] _defaultAppearSounds;
    private AudioClip[] _defaultHitSounds;
    private AudioClip[] _defaultCaptureSounds;


    private void Start()
    {
        _defaultAppearSounds = _appearSounds.Clips;
        _defaultHitSounds = _hitSounds.Clips;
        _defaultCaptureSounds = _hitSounds.Clips;

        foreach (RandomSoundEffectPlayer player in new[]{
            _throwSounds, _restSounds, _wakeupSounds,
            _appearSounds, _hitSounds, _captureSounds
        })
            player.CreateAudioSource(gameObject);

        BattleEventBus.SphereThrown += _throwSounds.Play;
        BattleEventBus.RestPeriodStarted += _restSounds.Play;
        BattleEventBus.RestPeriodEnded += _wakeupSounds.Play;
        BattleEventBus.MonsterAppeared += OnMonsterAppeared;
        BattleEventBus.MonsterHit += _hitSounds.Play;
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    }

    private void OnDestroy()
    {
        BattleEventBus.SphereThrown -= _throwSounds.Play;
        BattleEventBus.RestPeriodStarted -= _restSounds.Play;
        BattleEventBus.RestPeriodEnded -= _wakeupSounds.Play;
        BattleEventBus.MonsterAppeared -= OnMonsterAppeared;
        BattleEventBus.MonsterHit -= _hitSounds.Play;
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;
    }

    private void OnMonsterAppeared(MonsterData monster)
    {
        _appearSounds.Clips = monster.AppearSounds.Coalesce(_defaultAppearSounds);
        _hitSounds.Clips = monster.HitSounds.Coalesce(_defaultHitSounds);
        _appearSounds.Play();
    }

    private void OnMonsterCaptured(MonsterData monster)
    {
        _captureSounds.Clips = monster.CaptureSounds.Coalesce(_defaultCaptureSounds);
        _captureSounds.Play();
    }


    [System.Serializable]
    public class RandomSoundEffectPlayer
    {
        [Range(0, 1)] public float Volume = 0.5f;
        public AudioClip[] Clips;

        private AudioSource _source;


        public void Play()
        {
            if (!_source || Clips.Length == 0) return;
            _source.clip = Clips.PickRandom();
            _source.volume = Volume;
            _source.Play();
        }

        public void CreateAudioSource(GameObject hostObject)
        {
            _source = hostObject.AddComponent<AudioSource>();
        }
    }
}