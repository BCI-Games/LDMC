using System;
using UnityEngine;

public class BlockManager: MonoBehaviour
{
    public static bool IsOnBlock = false;

    private float _timer = 0;


    private void Start() => StartOffBlock();
    private void Update()
    {
        _timer += Time.deltaTime;
        if (IsOnBlock)
            CheckTimer(Settings.OnBlockDuration, StartOffBlock);
        else    
            CheckTimer(Settings.OffBlockDuration, StartOnBlock);
    }

    private void CheckTimer(float blockDuration, Action startNextBlockMethod)
    {
        if (_timer > blockDuration)
        {
            _timer -= blockDuration;
            startNextBlockMethod();
        }
    }


    private void StartOffBlock()
    {
        IsOnBlock = false;
        BattleEventBus.NotifyOffBlockStarted();
    }

    private void StartOnBlock()
    {
        IsOnBlock = true;
        BattleEventBus.NotifyOnBlockStarted();
    }
}