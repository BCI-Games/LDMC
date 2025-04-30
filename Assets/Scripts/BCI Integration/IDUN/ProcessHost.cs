using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class ProcessManager: MonoBehaviour
{
    public event Action<string> OutputLineReceived;
    public event Action<string> ErrorLineReceived;

    [Tooltip("filepath of the target application relative to the streaming assets folder")]
    public string ApplicationPath;
    public string[] Arguments;

    [Header("Debug")]
    public bool LogOutput;
    public string LogLabel = "Process Output";

    public bool ProcessRunning => _process != null && !_process.HasExited;
    private Process _process;

    private bool _isLoggingMethodSubscribed;


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
            return;
        }
        if (_process == null)
        {
            string qualifiedApplicationPath = Path.Combine(
                Application.streamingAssetsPath, ApplicationPath
            );
            if (!File.Exists(qualifiedApplicationPath))
            {
                Debug.LogWarning("Target application not found");
                return;
            }

            BuildProcess(qualifiedApplicationPath);
        }

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
        _process.OutputDataReceived += SendOutputEvent;
        _process.ErrorDataReceived += SendErrorEvent;

        if (LogOutput)
        {
            OutputLineReceived += LogOutputLine;
            _isLoggingMethodSubscribed = true;
        }
    }

    private void BuildProcess(string qualifiedPath)
    {
        _process = new()
        {
            StartInfo = new(qualifiedPath)
            {
                Arguments = string.Join(' ', Arguments),
                ErrorDialog = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        };
    }

    [ContextMenu("End Process")]
    public void EndProcess()
    {
        if (!ProcessRunning)
        {
            Debug.LogWarning("Process isn't running, ignoring...");
            return;
        }

        if (_isLoggingMethodSubscribed)
        {
            OutputLineReceived -= LogOutputLine;
            _isLoggingMethodSubscribed = false;
        }

        _process.OutputDataReceived -= SendOutputEvent;
        _process.ErrorDataReceived -= SendErrorEvent;
        _process.CancelOutputRead();
        _process.CancelErrorRead();

        _process.Kill();
    }


    private void SendOutputEvent(object sender, DataReceivedEventArgs args)
    => OutputLineReceived?.Invoke(args.Data);

    private void SendErrorEvent(object sender, DataReceivedEventArgs args)
    => ErrorLineReceived?.Invoke(args.Data);

    private void LogOutputLine(string outputLine)
    => Debug.Log($"{LogLabel}: {outputLine}");
}