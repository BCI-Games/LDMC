using System;

public static class BattleEventBus
{
    public static event Action<MonsterData> MonsterAppeared;
    public static void NotifyMonsterAppeared(MonsterData monsterData)
        => MonsterAppeared?.Invoke(monsterData);

    public static event Action WindupStarted;
    public static void NotifyWindupStarted()
        => WindupStarted?.Invoke();

    public static event Action WindupCancelled;
    public static void NotifyWindupCancelled()
        => WindupCancelled?.Invoke();

    public static event Action SphereThrown;
    public static void NotifySphereThrown()
        => SphereThrown?.Invoke();

    public static event Action LastSphereThrown;
    public static void NotifyLastSphereThrown()
        => LastSphereThrown?.Invoke();

    public static event Action MonsterHit;
    public static void NotifyMonsterHit()
        => MonsterHit?.Invoke();

    public static event Action<MonsterData> MonsterCaptured;
    public static void NotifyMonsterCaptured(MonsterData monsterData)
        => MonsterCaptured?.Invoke(monsterData);

    
    public static event Action RestPeriodStarted;
    public static void NotifyRestPeriodStarted()
        => RestPeriodStarted?.Invoke();
    public static event Action RestPeriodEnded;
    public static void NotifyRestPeriodEnded()
        => RestPeriodEnded?.Invoke();


    public static event Action<bool> PauseToggled;
    public static void NotifyPauseToggled(bool isPaused)
        => PauseToggled?.Invoke(isPaused);    
}