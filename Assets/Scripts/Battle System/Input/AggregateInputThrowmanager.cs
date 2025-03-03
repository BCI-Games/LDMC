using UnityEngine;

public class AggregateInputThrowManager: ThrowManager
{
    private IInputProvider[] _inputProviders;
    private float NormalizedInputValue => GetNormalizedInputValue();


    protected override void Start()
    {
        base.Start();
        _inputProviders = GetComponents<IInputProvider>();
    }


    private float GetNormalizedInputValue()
    {
        if (_inputProviders == null) return 0;

        float totalInputValue = 0;
        foreach(var inputProvider in _inputProviders)
        {
            totalInputValue += Mathf.Clamp01(inputProvider.InputValue);
        }
        return Mathf.Clamp01(totalInputValue);
    }


    protected override void AddFrameTimeToChargeLevel()
    => ChargeLevel += NormalizedInputValue * Time.deltaTime / ChargePeriod;

    protected override bool GetShouldCharge()
    => NormalizedInputValue > 0;
}

public interface IInputProvider
{
    public abstract float InputValue {get;}
}