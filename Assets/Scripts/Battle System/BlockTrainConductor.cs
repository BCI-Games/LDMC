using System;
using UnityEngine;

public class BlockTrainConductor: MonoBehaviour
{
    public static event Action OnBlockStarted;
    public static event Action OffBlockStarted;

    private enum State { Inactive, OffBlock, OnBlock }
    private State _state = State.Inactive;
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
        if (_state == State.Inactive) return;

        _timer += Time.deltaTime;
        if (_state == State.OnBlock)
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
        if (!isPaused)
        {
            StartOffBlock();
            BattleEventBus.PauseToggled -= OnPauseToggled;
        }
    }

    private void StartOffBlock()
    {
        _state = State.OffBlock;
        OffBlockStarted?.Invoke();
    }

    private void StartOnBlock()
    {
        _state = State.OnBlock;
        OnBlockStarted?.Invoke();
    }
}