using System.Collections;
using UnityEngine;

public class BlockManager: MonoBehaviour
{
    public static bool IsOnBlock = false;


    private void Start()
    {
        BattleEventBus.LastSphereThrown += StartOffBlock;
        StartOffBlock();
    }
    
    private void OnDestroy() => BattleEventBus.LastSphereThrown -= StartOffBlock;


    private void StartOffBlock()
    {
        IsOnBlock = false;
        BattleEventBus.NotifyOffBlockStarted();
        StartCoroutine(StartOnBlockAfterDelay(Settings.OffBlockDuration));
    }

    private IEnumerator StartOnBlockAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsOnBlock = true;
        BattleEventBus.NotifyOnBlockStarted();
    }
}