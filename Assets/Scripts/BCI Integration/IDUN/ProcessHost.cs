using UnityEngine;
using System.Diagnostics;
using System.IO;

using Debug = UnityEngine.Debug;

public class ProcessManager: MonoBehaviour
{
    public event DataReceivedEventHandler OutputDataReceived;
    public event DataReceivedEventHandler ErrorDataReceived;


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
            OutputDataReceived += (sender, args) =>
            Debug.Log("process Data received: " + args.Data);
        }
    }

    void OnDestroy()
    {
        if (ProcessRunning) EndProcess();
    }

    [ContextMenu("Start Process")]
    public void StartProcess()
    {
        string fullApplicationPath = Path.Combine(Application.streamingAssetsPath, ApplicationPath);
        if (!File.Exists(fullApplicationPath))
        {
            Debug.LogWarning("Target application not found");
            return;
        }
        _process = Process.Start(fullApplicationPath, string.Join(' ', Arguments));
        _process.OutputDataReceived += BindDataHandler(OutputDataReceived);
        _process.ErrorDataReceived += BindDataHandler(ErrorDataReceived);
    }

    [ContextMenu("End Process")]
    public void EndProcess()
    {
        if (!ProcessRunning)
        {
            Debug.LogWarning("Process isn't running");
        }
        _process.Kill();
    }

    private DataReceivedEventHandler BindDataHandler(DataReceivedEventHandler dataEvent)
    => (sender, dataArgs) => dataEvent?.Invoke(sender, dataArgs);
}