using UnityEngine;
using System.Collections;

public class AutomatedThrowManager: ThrowManager
{
    [SerializeField] private float _throwDelay = 0.5f;

    private bool _isThrowing;
    private Coroutine _autoThrowCoroutine;


    protected override void Start()
    {
        base.Start();
        BattleEventBus.ActiveBlockStarted += StartAutoThrow;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleEventBus.ActiveBlockStarted -= StartAutoThrow;
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
            ThrowSphere();
            _chargeLevel = 0;
            yield return new WaitForSeconds(_throwDelay);
            BattleEventBus.NotifyWindupStarted();
            while (_chargeLevel < 1)
            {
                AddFrameTimeToChargeLevel();
                yield return new WaitForEndOfFrame();
            }
        }
        _isThrowing = false;
    }
}