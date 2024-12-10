using UnityEngine;
using System.Collections;

public class AutomatedThrowManager: ThrowManager
{
    private float _throwDelay = 0.5f;

    private bool _isThrowing;
    private Coroutine _autoThrowCoroutine;


    protected override void Start()
    {
        base.Start();
        Settings.AddAndInvokeModificationCallback(UpdateParametersFromSettings);
        BattleEventBus.OnBlockStarted += StartAutoThrow;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Settings.Modified -= UpdateParametersFromSettings;
        BattleEventBus.OnBlockStarted -= StartAutoThrow;
    }

    private void UpdateParametersFromSettings()
    {
        _chargePeriod = Settings.CharacterActiveDuration;
        _throwDelay = Settings.CharacterIdleDuration;
        _sphereCount = Settings.OnBlockCycleCount;
    }

    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            if(_isThrowing) StopAutoThrow(); else StartAutoThrow();
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
            _chargeLevel = 0;
        }
    }

    private IEnumerator RunAutoThrow()
    {
        _isThrowing = true;
        while(_numberOfSpheresRemaining > 0)
        {
            BattleEventBus.NotifyWindupStarted();
            while (_chargeLevel < 1)
            {
                AddFrameTimeToChargeLevel();
                yield return new WaitForEndOfFrame();
            }
            ThrowSphere();
            _chargeLevel = 0;
            yield return new WaitForSeconds(_throwDelay);
        }
        _isThrowing = false;
    }
}