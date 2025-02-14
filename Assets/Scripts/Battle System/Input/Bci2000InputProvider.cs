using UnityEngine;
using BCI2000;

public class Bci2000InputProvider: MonoBehaviour, IBooleanInputProvider
{
    public bool InputValue => _eventValue;

    public string EventName = "test";

    private Bci2000RemoteProxy _bci2000Proxy;
    private bool _eventValue = false;


    private void Start()
    {
        _bci2000Proxy = GetComponent<Bci2000RemoteProxy>();
        
    }

    private void Update()
    {
        _eventValue = _bci2000Proxy.GetEvent(EventName) > 0;
    }
}