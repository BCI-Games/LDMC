using UnityEngine;
using BCI2000;

public class BCI2000StateManager: MonoBehaviour
{
    enum StateValue: uint {
        StartTask = 90,
        EndTask = 91,
        Rest = 120,
        RestMotion = 128,
        Active = 110,
        ActiveMotion = 124,
        Input = 126,
        Iti = 7
    };

    [SerializeField] private BCI2000RemoteProxy _bci2000Proxy;


    void Reset()
    {
        _bci2000Proxy = GetComponent<BCI2000RemoteProxy>();
    }

    void Start()
    {
        _bci2000Proxy ??= FindAnyObjectByType<BCI2000RemoteProxy>();

        _bci2000Proxy.OperatorConnected += () => {
            SetPhase(true);
            SetCodeAndCondition(StateValue.StartTask);
        };

        BattleEventBus.RestPeriodStarted += OnRestPeriodStarted;
        BattleEventBus.RestPeriodEnded += OnRestPeriodEnded;
        BattleEventBus.WindupStarted += OnWindupStarted;
        BattleEventBus.WindupCancelled += OnWindupCancelled;
        BattleEventBus.SphereThrown += OnSphereThrow;
        BattleEventBus.MonsterCaptured += OnMonsterCaught;
    }

    void OnDestroy()
    {
        SetPhase(false);
        SetCodeAndCondition(StateValue.EndTask);

        BattleEventBus.RestPeriodStarted -= OnRestPeriodStarted;
        BattleEventBus.RestPeriodEnded -= OnRestPeriodEnded;
        BattleEventBus.WindupStarted -= OnWindupStarted;
        BattleEventBus.WindupCancelled -= OnWindupCancelled;
        BattleEventBus.SphereThrown -= OnSphereThrow;
        BattleEventBus.MonsterCaptured -= OnMonsterCaught;
    }


    void OnRestPeriodStarted() => SetCodeAndCondition(StateValue.Rest);
    void OnRestPeriodEnded() => SetCodeAndCondition(StateValue.Active);

    void OnWindupStarted() => SetCode(StateValue.ActiveMotion);
    void OnWindupCancelled() => SetCode(StateValue.Active);
    void OnSphereThrow() => SetCode(StateValue.Active);
    
    void OnMonsterCaught(MonsterData monsterData)
    => SetCodeAndCondition(StateValue.Iti);
    

    private void SetPhase(bool value)
    => SetState("phase", (uint)(value? 1: 0));

    private void SetCodeAndCondition(StateValue value)
    {
        SetCode(value);
        SetState("condition", value);
    }
    private void SetCode(StateValue value)
    => SetState("code", value);

    private void SetState(string name, StateValue value)
    => SetState(name, (uint) value);
    private void SetState(string name, uint value)
    {
        if (_bci2000Proxy && _bci2000Proxy.Connected())
            _bci2000Proxy.SetState(name, value);
    }
}
