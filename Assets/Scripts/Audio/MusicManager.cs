using UnityEngine;
using System.Linq;

public class MusicManager: MonoBehaviour
{
    const string TrackListPath = "Audio/Track List";
    public static TrackList Tracks => _tracks ??= LoadTrackList();
    private static TrackList _tracks;

    private AudioSource _source;
    private int _activeTrackIndex = -1;
    

    private void Start()
    {
        _source = gameObject.AddComponent<AudioSource>();
        _source.outputAudioMixerGroup = VolumeManager.GlobalAudioMixer.FindMatchingGroups("Music")[0];
        _source.loop = true;

        Settings.AddAndInvokeModificationCallback(SelectTrackFromSettings);
    }
    private void OnDestroy() => Settings.Modified -= SelectTrackFromSettings;

    private void SelectTrackFromSettings()
    {
        int trackIndex = Settings.MusicTrackIndex;
        if (trackIndex == _activeTrackIndex) return;

        AudioClip[] audioClips = Tracks.AudioFiles;

        trackIndex = Mathf.Clamp(trackIndex, 0, audioClips.Length);
        _source.clip = audioClips[trackIndex];
        _source.Play();
        _activeTrackIndex = trackIndex;
    }

    private static TrackList LoadTrackList() => Resources.Load<TrackList>(TrackListPath);
}