using UnityEngine;
using System.IO;

public static partial class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.dataPath + "/../" + FileName;
    

    public static void LoadAndApplySettings()
    {
        if (_container) return;
        _container = LoadContainer();
        ApplyModifiedValue();
    }


    private static SettingsContainer LoadContainer()
    {
        SettingsContainer loadedSettings = new();
        if (File.Exists(FilePath))
        {
            StreamReader reader = new(FilePath);
            string fileContent = reader.ReadToEnd();
            reader.Close();

            JsonUtility.FromJsonOverwrite(fileContent, loadedSettings);
        }
        SaveContainer(loadedSettings);
        if (!_audioManager) InitializeAudioManager();
        return loadedSettings;
    }

    private static void InitializeAudioManager()
    {
        _audioManager = new("Audio Manager");
        _audioManager.AddComponent<VolumeManager>();
        _audioManager.AddComponent<MusicManager>();
        GameObject.DontDestroyOnLoad(_audioManager);
    }


    private static void SaveContainer(SettingsContainer container)
    {
        string fileContent = JsonUtility.ToJson(container, true);
        StreamWriter writer = new(FilePath);
        writer.Write(fileContent);
        writer.Close();
    }
}