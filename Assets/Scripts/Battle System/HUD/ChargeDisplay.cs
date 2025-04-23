using UnityEngine;

public abstract class ChargeDisplay: MonoBehaviour
{
    public float ChargeLevel{
        get => _chargeLevel;
        set => SetChargeLevel(value);
    }
    protected float _chargeLevel;

    protected abstract void SetChargeLevel(float value);
}