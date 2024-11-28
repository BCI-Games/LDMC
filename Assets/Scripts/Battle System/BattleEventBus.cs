using System;

public static class BattleEventBus
{
    public static event Action<MonsterData> MonsterAppeared;
    public static void NotifyMonsterAppeared(MonsterData monsterData)
        => MonsterAppeared?.Invoke(monsterData);

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

    public static event Action PlayerTurnStarted;
    public static void NotifyPlayerTurnStarted()
        => PlayerTurnStarted?.Invoke();

    public static event Action OpponentTurnStarted;
    public static void NotifyOpponentTurnStarted()
        => OpponentTurnStarted?.Invoke();
}
