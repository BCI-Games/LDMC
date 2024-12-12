using UnityEngine;
using System.Linq;

public class MusicManager: MonoBehaviour
{
    private static MusicManager instance = null;

    public static string[] TrackNames => GetTrackNames();

    [SerializeField] AudioClip[] _musicTracks;

    private AudioSource _source;
    private int _activeTrackIndex;
    

    private void Start()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        _source = GetComponent<AudioSource>();
        Settings.AddAndInvokeModificationCallback(SelectTrackFromSettings);
    }
    private void OnDestroy() => Settings.Modified -= SelectTrackFromSettings;

    private void SelectTrackFromSettings()
    {
        int trackIndex = Settings.MusicTrackIndex;
        if (trackIndex == _activeTrackIndex) return;

        trackIndex = Mathf.Clamp(trackIndex, 0, _musicTracks.Length);
        _source.clip = _musicTracks[trackIndex];
        _source.Play();
        _activeTrackIndex = trackIndex;
    }

    private static string[] GetTrackNames()
    {
        if (!instance) return new string[0];
        return instance._musicTracks.Select((AudioClip clip) => clip.name).ToArray();
    }
}