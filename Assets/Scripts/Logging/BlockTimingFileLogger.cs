using System;
using System.Text;
using System.IO;
using UnityEngine;

public class BlockTimingFileLogger: MonoBehaviour
{
    [SerializeField] private string FolderName = "BlockTimingLogs";
    [SerializeField] private string BaseFileName = "block onsets";
    [SerializeField] private string FileNameTimestampTemplate = "dd/MM/yyyy HH:mm";
    [SerializeField] private string TimestampTemplate = "HH:mm:ss.fff";

    private string filePath = null;


    void Start()
    {
        BattleEventBus.OnBlockStarted += LogOnBlockOnset;
        BattleEventBus.OffBlockStarted += LogOffBlockOnset;
    }

    void OnDestroy()
    {
        BattleEventBus.OnBlockStarted -= LogOnBlockOnset;
        BattleEventBus.OffBlockStarted -= LogOffBlockOnset;
    }


    private void LogOnBlockOnset() => LogBlockOnset("On");
    private void LogOffBlockOnset() => LogBlockOnset("Off");

    private void LogBlockOnset(string blockLabel)
    {
        if (filePath == null)
            InitializeLogFile();

        StringBuilder logString = new StringBuilder("\n");
        logString.Append(GetTimestamp());
        logString.Append(",");
        logString.Append(blockLabel);

        File.AppendAllText(filePath, logString.ToString());
    }


    private void InitializeLogFile()
    {
        string directoryPath = Application.dataPath;
        directoryPath += "/../" + FolderName;
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        filePath = $"{directoryPath}/{BaseFileName} ";
        filePath += DateTime.Now.ToString(FileNameTimestampTemplate);
        filePath += ".csv";

        File.WriteAllText(filePath, "Timestamp, Event");
    }

    private string GetTimestamp()
        => DateTime.Now.ToString(TimestampTemplate);
}