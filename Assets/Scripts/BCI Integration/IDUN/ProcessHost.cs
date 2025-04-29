using UnityEngine;
using System.Diagnostics;
using System.IO;

using Debug = UnityEngine.Debug;

public class ProcessManager: MonoBehaviour
{
    [Tooltip("filepath of the target application relative to the streaming assets folder")]
    public string ApplicationPath;
    public string[] Arguments;

    public bool ProcessRunning => _process != null && !_process.HasExited;
    private Process _process;


    void OnDestroy()
    {
        if (ProcessRunning) EndProcess();
    }


    [ContextMenu("Start Process")]
    public void StartProcess()
    {
        if (ProcessRunning)
        {
            Debug.LogWarning("Process is already running, ignoring...");
        }
        else if (_process != null) _process.Start();
        else
        {
            string fullApplicationPath = Path.Combine(
                Application.streamingAssetsPath, ApplicationPath
            );
            if (!File.Exists(fullApplicationPath))
            {
                Debug.LogWarning("Target application not found");
                return;
            }

            string argumentsString = string.Join(' ', Arguments);
            ProcessStartInfo startInfo = new(fullApplicationPath, argumentsString)
            {
                CreateNoWindow = true, UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            _process = Process.Start(startInfo);
        }
    }

    [ContextMenu("End Process")]
    public void EndProcess()
    {
        if (!ProcessRunning)
        {
            Debug.LogWarning("Process isn't running, ignoring...");
            return;
        }

        foreach (Process p in Process.GetProcessesByName(_process.ProcessName))
            p.Kill();
    }
}