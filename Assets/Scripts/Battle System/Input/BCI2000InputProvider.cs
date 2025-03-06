using UnityEngine;
using BCI2000;

public class BCI2000InputProvider: MonoBehaviour, IInputProvider
{
    public float InputValue => _eventValue;

    public string EventName = "test";

    private BCI2000RemoteProxy _bci2000Proxy;
    private float _eventValue = 0;


    private void Start()
    {
        _bci2000Proxy = GetComponent<BCI2000RemoteProxy>();
        
    }

    private void Update()
    {
        if (_bci2000Proxy.Connected())
            _eventValue = _bci2000Proxy.GetEvent(EventName);
    }
}