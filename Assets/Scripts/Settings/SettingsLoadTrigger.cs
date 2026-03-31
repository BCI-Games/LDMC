using UnityEngine;

public class SettingsLoadTrigger: MonoBehaviour
{
    private static bool _loadTriggered;

    private void Awake()
    {
        if (!_loadTriggered)
        {
            Settings.LoadAndApplySettings();

            GameObject audioManager = new("Audio Manager");
            audioManager.AddComponent<VolumeManager>();
            audioManager.AddComponent<MusicManager>();
            DontDestroyOnLoad(audioManager);

            _loadTriggered = true;
        }

        Destroy(gameObject);
    }
}