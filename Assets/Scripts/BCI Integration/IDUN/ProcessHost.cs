using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

using Debug = UnityEngine.Debug;

public class ProcessManager: MonoBehaviour
{
    public event Action<string> OutputDataReceived;
    public event Action<string> ErrorDataReceived;

    [Tooltip("filepath of the target application relative to the streaming assets folder")]
    public string ApplicationPath;
    public string[] Arguments;
    public bool LogOutput;

    public bool ProcessRunning => _process != null && !_process.HasExited;
    private Process _process;


    void Start()
    {
        if (LogOutput)
        {
            OutputDataReceived += dataString =>
                Debug.Log("Output data received: " + dataString);
        }
    }

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
                CreateNoWindow = false, UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            _process = Process.Start(startInfo);
            _process.OutputDataReceived += BindDataHandler(OutputDataReceived);
            _process.ErrorDataReceived += BindDataHandler(ErrorDataReceived);
            _process.BeginOutputReadLine();

            if (LogOutput)
            {
                _process.OutputDataReceived += (sender, data)
                => Debug.Log("Output data received directly: " + data.Data);
            }
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

    private DataReceivedEventHandler BindDataHandler(Action<string> dataEvent)
    => (sender, dataArgs) => dataEvent?.Invoke(dataArgs.Data);
}