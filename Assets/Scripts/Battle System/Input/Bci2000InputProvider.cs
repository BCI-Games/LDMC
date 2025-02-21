using UnityEngine;
using BCI2000;

public class Bci2000InputProvider: MonoBehaviour, IBooleanInputProvider
{
    public bool InputValue => _eventValue;

    public string EventName = "test";

    private BCI2000RemoteProxy _bci2000Proxy;
    private bool _eventValue = false;


    private void Start()
    {
        _bci2000Proxy = GetComponent<BCI2000RemoteProxy>();
        
    }

    private void Update()
    {
        if (_bci2000Proxy.Connected())
            _eventValue = _bci2000Proxy.GetEvent(EventName) > 0;
    }
}