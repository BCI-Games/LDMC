using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    public MonsterData[] Monsters;
    
    private MonsterPresenter _monsterPresenter;
    private MonsterData _monsterExcludedFromNextSpawn;
    private bool _monsterSpawnQueued = false;


    void Start()
    {
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
        BattleEventBus.RestPeriodEnded += SpawnNewMonsterIfPending;
        _monsterPresenter = GetComponentInChildren<MonsterPresenter>();

        if (Settings.OffBockMonsterDisplayEnabled)
            SpawnMonster(Monsters.PickRandom());
        else
            QueueMonsterSpawn();
    }

    private void OnDestroy()
    {
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;
        BattleEventBus.RestPeriodEnded -= SpawnNewMonsterIfPending;
    }


    private void OnMonsterCaptured(MonsterData capturedMonsterData)
    {
        if (Settings.OffBockMonsterDisplayEnabled)
            SpawnNewMonster(capturedMonsterData);
        else
            QueueMonsterSpawn(capturedMonsterData);
    }

    private void SpawnNewMonsterIfPending()
    {
        if (_monsterSpawnQueued)
        {
            SpawnNewMonster(_monsterExcludedFromNextSpawn);
            _monsterSpawnQueued = false;
        }
    }

    private void QueueMonsterSpawn(MonsterData excludedMonster = null)
    {
        _monsterPresenter.HideMonster();
        _monsterExcludedFromNextSpawn = excludedMonster;
        _monsterSpawnQueued = true;
    }

    private void SpawnNewMonster(MonsterData excludedMonster)
    {
        SpawnMonster(
            !excludedMonster ? Monsters.PickRandom()
            : Monsters.PickRandomExcluding(excludedMonster)
        );
    }

    private void SpawnMonster(MonsterData newMonster)
    {
        _monsterPresenter.ShowNewMonster(newMonster);
        BattleEventBus.NotifyMonsterAppeared(newMonster);
    }
}
