using UnityEngine;
using System.Collections;

public abstract class CharacterPresenter: MonoBehaviour
{
    private Coroutine _restCycleCoroutine;


    protected virtual void Start() => SubscribeToBattleEvents();
    protected virtual void OnDestroy() => UnsubscribeFromBattleEvents();

    protected void SubscribeToBattleEvents()
    {
        BattleEventBus.WindupStarted += ShowWindupStarted;
        BattleEventBus.WindupCancelled += ShowWindupCancelled;
        BattleEventBus.SphereThrown += ShowThrow;

        BattleEventBus.RestPeriodStarted += StartRestCycle;
        BattleEventBus.RestPeriodEnded += EndRestCycle;
    }

    protected void UnsubscribeFromBattleEvents()
    {
        BattleEventBus.WindupStarted -= ShowWindupStarted;
        BattleEventBus.WindupCancelled -= ShowWindupCancelled;
        BattleEventBus.SphereThrown -= ShowThrow;

        BattleEventBus.RestPeriodStarted -= StartRestCycle;
        BattleEventBus.RestPeriodEnded -= EndRestCycle;
    }

    protected abstract void ShowWindupStarted();
    protected abstract void ShowWindupCancelled();
    protected abstract void ShowThrow();
    protected abstract void ShowRestEnded();
    protected abstract void ShowRestStarted();

    private void StartRestCycle()
    {
        ShowRestStarted();
        _restCycleCoroutine = StartCoroutine(WrapRestCycle());
    }
    private void EndRestCycle()
    {
        StopCoroutine(_restCycleCoroutine);
        ShowRestEnded();
    }
    private IEnumerator WrapRestCycle()
    {
        while(true) yield return RunRestCycle();
    }
    protected abstract IEnumerator RunRestCycle();
}