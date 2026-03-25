using UnityEngine;

public class SettingsLoadTrigger: MonoBehaviour
{
    private void Awake()
    {
        Settings.LoadAndApplySettings();

        GameObject audioManager = new("Audio Manager");
        audioManager.AddComponent<VolumeManager>();
        audioManager.AddComponent<MusicManager>();
        DontDestroyOnLoad(audioManager);

        Destroy(gameObject);
    }
}