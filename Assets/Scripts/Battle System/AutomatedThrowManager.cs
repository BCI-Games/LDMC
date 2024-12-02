using UnityEngine;
using System.Collections;

public class AutomatedThrowManager: ThrowManager
{
    [SerializeField] private float _autoThrowDelay = 1;

    private bool _isAutoThrowing;
    private Coroutine _autoThrowCoroutine;


    protected override void Start()
    {
        base.Start();
        BattleEventBus.PlayerTurnStarted += StopAutoThrow;
        BattleEventBus.OpponentTurnStarted += StopAutoThrow;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        BattleEventBus.PlayerTurnStarted -= StopAutoThrow;
        BattleEventBus.OpponentTurnStarted -= StopAutoThrow;
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.R))
            ResetInventory();

        if(Input.GetKeyDown(KeyCode.A))
            StartAutoThrow();
    }


    public void StartAutoThrow()
    {
        StopAutoThrow();
        _autoThrowCoroutine = StartCoroutine(RunAutoThrow());
    }

    public void StopAutoThrow()
    {
        if (_isAutoThrowing) StopCoroutine(_autoThrowCoroutine);
    }

    private IEnumerator RunAutoThrow()
    {
        _isAutoThrowing = true;
        while(_numberOfSpheresRemaining > 0)
        {
            ThrowSphere();
            yield return new WaitForSeconds(_autoThrowDelay);
        }
        _isAutoThrowing = false;
    }
}