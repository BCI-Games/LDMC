using UnityEngine;
using System.Collections;

public class RestPeriodConductor: MonoBehaviour
{
    private void Start()
    {
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    }
    private void OnDestroy()
    {
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;
    }

    private void OnMonsterCaptured(MonsterData capturedMonster)
    {
        BattleEventBus.NotifyRestPeriodStarted();
        StartCoroutine(RunRestPeriod());
    }

    private IEnumerator RunRestPeriod()
    {
        yield return new WaitForSeconds(Settings.OffBlockDuration);
        BattleEventBus.NotifyRestPeriodEnded();
    }
}