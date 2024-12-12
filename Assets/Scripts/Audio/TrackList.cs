using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Track List", menuName = "TrackList")]
public class TrackList : ScriptableObject
{
    public NamedTrack[] Tracks;

    public string[] Names => Tracks.Select(namedTrack => namedTrack.ShortName).ToArray();
    public AudioClip[] AudioFiles => Tracks.Select(namedTrack => namedTrack.AudioFile).ToArray();
}

[Serializable]
public struct NamedTrack
{
    public string ShortName;
    public AudioClip AudioFile;
}