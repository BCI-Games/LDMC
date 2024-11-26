using System;

public static class BattleEventBus: Object
{
    public static event Action<MonsterData> MonsterAppeared;
    public static void NotifyMonsterAppeared(MonsterData monsterData)
        => MonsterAppeared?.Invoke(monsterData);

    public static event Action SphereThrown;
    public static void NotifySphereThrown()
        => SphereThrown?.Invoke();

    public static event Action MonsterHit;
    public static void NotifyMonsterHit()
        => MonsterHit?.Invoke();

    public static event Action MonsterBecameCatchable;
    public static void NotifyMonsterCatchable()
        => MonsterBecameCatchable?.Invoke();

    public static event Action<MonsterData> MonsterCaptured;
    public static void NotifyMonsterCaptured(MonsterData monsterData)
        => MonsterCaptured?.Invoke(monsterData);
}
