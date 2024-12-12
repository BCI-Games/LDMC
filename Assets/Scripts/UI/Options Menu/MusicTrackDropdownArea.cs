using UnityEngine;
using TMPro;
using System.Linq;

public class MusicTrackDropdown: MonoBehaviour
{
    private void Start()
    {
        TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();

        dropdown.options = MusicManager.Tracks.Names.Select
            ((string name) => new TMP_Dropdown.OptionData(name)).ToList();

        dropdown.value = Settings.MusicTrackIndex;

        dropdown.onValueChanged.AddListener(SelectMusicTrack);
    }

    private void SelectMusicTrack(int index) => Settings.MusicTrackIndex = index;
}