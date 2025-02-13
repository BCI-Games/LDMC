using UnityEngine;
using BCI2000;
using BCIEssentials.Controllers;

[RequireComponent(typeof(BCI2000Controller))]
public class Bci2000InputProvider: MonoBehaviour, IBooleanInputProvider
{
    public bool InputValue => _eventValue;

    public string EventName = "test";

    private BCI2000Controller _controller;
    private bool _eventValue = false;


    private void Start()
    {
        _controller = GetComponent<BCI2000Controller>();
        
    }

    private void Update()
    {
        _eventValue = _controller.GetEvent(EventName) > 0;
    }
}