using UnityEngine;
using System.IO;

public static partial class Settings
{
    const string FileName = "settings.json";
    private static string FilePath => Application.dataPath + "/../" + FileName;
    private static GameObject _audioManager;


    public static void LoadAndApplySettings()
    {
        LoadContainer().ApplyValues();
        Modified?.Invoke();
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
        ConnectModificationEvents();
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

    private static void ConnectModificationEvents()
    {
        ValueProxy.InstanceModified += () =>
        {
            SaveContainer(new());
            Modified?.Invoke();
        };
    }


    private static void SaveContainer(SettingsContainer container)
    {
        string fileContent = JsonUtility.ToJson(container, true);
        StreamWriter writer = new(FilePath);
        writer.Write(fileContent);
        writer.Close();
    }
}