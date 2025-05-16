using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [Range(0, 1)] public float Volume = 0.5f;
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _source;

    public void Play()
    {
        if (!_source) CreateAudioSource();
        if (_clips.Length == 0) return;
        _source.clip = _clips.PickRandom();
        _source.volume = Volume;
        _source.Play();
    }

    private void CreateAudioSource()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }
}