using UnityEngine;
using TMPro;
using System.Linq;

public class MusicTrackDropdown: MonoBehaviour
{
    private void Start()
    {
        TMP_Dropdown dropdown = GetComponentInChildren<TMP_Dropdown>();

        dropdown.options = MusicManager.Tracks.Names.Select
            (name => new TMP_Dropdown.OptionData(name)).ToList();

        Settings.MusicTrackIndex.ConnectDropdown(dropdown);
    }
}