using UnityEngine;

public class AggregateInputThrowManager: ThrowManager
{
    private IBooleanInputProvider[] _inputProviders;


    protected override void Start()
    {
        base.Start();
        _inputProviders = GetComponents<IBooleanInputProvider>();
    }


    protected override bool GetShouldCharge()
    {
        if (_inputProviders == null) return false;

        bool shouldCharge = false;
        foreach(var inputProvider in _inputProviders)
        {
            shouldCharge |= inputProvider.InputValue;
        }
        return shouldCharge;
    }
}

public interface IBooleanInputProvider
{
    public abstract bool InputValue {get;}
}