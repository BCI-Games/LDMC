using UnityEngine;
using System.Collections;

public class AutomatedThrowManager: ThrowManager
{
    private bool _isThrowing;
    private Coroutine _autoThrowCoroutine;

    private float _throwDelay;
    private bool _shouldCharge;


    protected override void Start()
    {
        base.Start();
        BattleEventBus.RestPeriodEnded += StartAutoThrow;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleEventBus.RestPeriodEnded -= StartAutoThrow;
    }

    protected override void UpdateParametersFromSettings()
    {
        base.UpdateParametersFromSettings();
        _throwDelay = Settings.CharacterIdleDuration;
    }


    public void StartAutoThrow()
    {
        StopAutoThrow();
        _autoThrowCoroutine = StartCoroutine(RunAutoThrow());
    }

    public void StopAutoThrow()
    {
        if (_isThrowing)
        {
            StopCoroutine(_autoThrowCoroutine);
            _isThrowing = false;
            if (IsCharging) BattleEventBus.NotifyWindupCancelled();
            ResetChargeLevel();
        }
    }

    private IEnumerator RunAutoThrow()
    {
        _isThrowing = true;
        while(SpheresRemain)
        {
            _shouldCharge = true;
            BattleEventBus.NotifyWindupStarted();
            yield return new WaitForSeconds(ChargePeriod);
            ThrowSphere();
            ResetChargeLevel();
            _shouldCharge = false;
            yield return new WaitForSeconds(_throwDelay);
        }
        _isThrowing = false;
    }
    
    protected override bool GetShouldCharge() => _shouldCharge;
}