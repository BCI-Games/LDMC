using System;
using UnityEngine;

public class BlockTrainConductor: MonoBehaviour
{
    public static bool IsOnBlock = false;
    public static event Action OnBlockStarted;
    public static event Action OffBlockStarted;

    private float _timer = 0;


    private void Start()
    {
        BattleEventBus.PauseToggled += OnPauseToggled;
        OffBlockStarted += BattleEventBus.NotifyRestPeriodStarted;
        OnBlockStarted += BattleEventBus.NotifyRestPeriodEnded;
    }
    private void OnDestroy()
    {
        BattleEventBus.PauseToggled -= OnPauseToggled;
        OffBlockStarted -= BattleEventBus.NotifyRestPeriodStarted;
        OnBlockStarted -= BattleEventBus.NotifyRestPeriodEnded;
    }


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


    private void OnPauseToggled(bool isPaused)
    {
        if (!isPaused) StartOffBlock();
        BattleEventBus.PauseToggled -= OnPauseToggled;
    }

    private void StartOffBlock()
    {
        IsOnBlock = false;
        OffBlockStarted?.Invoke();
    }

    private void StartOnBlock()
    {
        IsOnBlock = true;
        OnBlockStarted?.Invoke();
    }
}