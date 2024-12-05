using System.Collections;
using UnityEngine;

public class BlockManager: MonoBehaviour
{
    public static bool IsActiveBlock = false;


    private void Start()
    {
        BattleEventBus.LastSphereThrown += StartIdleBlock;
        StartIdleBlock();
    }
    
    private void OnDestroy() => BattleEventBus.LastSphereThrown -= StartIdleBlock;


    private void StartIdleBlock()
    {
        IsActiveBlock = false;
        BattleEventBus.NotifyIdleBlockStarted();
        StartCoroutine(StartActiveBlockAfterDelay(Settings.IdleBlockDuration));
    }

    private IEnumerator StartActiveBlockAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsActiveBlock = true;
        BattleEventBus.NotifyActiveBlockStarted();
    }
}