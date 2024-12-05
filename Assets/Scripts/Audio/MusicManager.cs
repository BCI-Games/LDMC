using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager: MonoBehaviour
{
    [SerializeField] AudioClip[] _musicTracks;

    private AudioSource _source;
    

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        Settings.AddAndInvokeModificationCallback(SelectTrackFromSettings);
    }
    private void OnDestroy() => Settings.Modified -= SelectTrackFromSettings;

    private void SelectTrackFromSettings()
    {
        int clipIndex = Settings.MusicTrackIndex;
        clipIndex = Mathf.Clamp(clipIndex, 0, _musicTracks.Length);
        _source.clip = _musicTracks[clipIndex];
        _source.Play();
    }
}