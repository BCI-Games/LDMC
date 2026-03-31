using UnityEngine;

public abstract class ChargeDisplay: MonoBehaviour
{
    public float ChargeLevel{
        get => _chargeLevel;
        set => SetChargeLevel(value);
    }
    protected float _chargeLevel;

    protected abstract void SetChargeLevel(float value);


    protected virtual void Start()
    {
        Hide();
        BattleEventBus.CaptureThresholdMet += Hide;
        BattleEventBus.RestPeriodEnded += Show;
    }

    protected virtual void OnDestroy()
    {
        BattleEventBus.CaptureThresholdMet -= Hide;
        BattleEventBus.RestPeriodEnded -= Show;
    }


    protected abstract void Hide();
    protected abstract void Show();
}