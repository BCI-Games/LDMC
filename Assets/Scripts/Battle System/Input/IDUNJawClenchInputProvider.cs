using UnityEngine;

public class IDUNJawClenchInputProvider: MonoBehaviour, IInputProvider
{
    public float InputValue => _lastPredictionValue? 1: 0;

    [SerializeField] private ProcessHost _processHost;
    [SerializeField] private UTF8SocketHost _socketHost;

    private bool _lastPredictionValue;


    void Start()
    {
        if (!_processHost || !_socketHost)
        {
            Debug.LogWarning("Missing references, aborting...");
            return;
        }
        _processHost.StartProcess();
        _socketHost.Open();
        _socketHost.MessageReceived += OnSocketMessageReceived;
    }

    void OnSocketMessageReceived(string message)
    {
        _lastPredictionValue = bool.Parse(message);
    }
}