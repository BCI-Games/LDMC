using System.Linq;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    public MonsterData[] Monsters;
    public bool EnableRarity;

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
        if (EnableRarity)
        {
            SpawnMonster(
                PickRandomMonsterWeightedByRarity(
                    !excludedMonster ? Monsters
                    : Monsters.Excluding(excludedMonster)
                )
            );
        }
        else
        {
            SpawnMonster(
                !excludedMonster ? Monsters.PickRandom()
                : Monsters.PickRandomExcluding(excludedMonster)
            );
        }
    }

    private void SpawnMonster(MonsterData newMonster)
    {
        _monsterPresenter.ShowNewMonster(newMonster);
        BattleEventBus.NotifyMonsterAppeared(newMonster);
    }

    public static MonsterData PickRandomMonsterWeightedByRarity(MonsterData[] monsters)
    {
        float[] edges = new float[monsters.Length];

        float totalRelativeSpawnFrequency = monsters.Sum(
            monsterData => 1.0f / monsterData.Rarity
        );

        float previousEdge = 0;
        for (int i = 0; i < edges.Length; i++)
        {
            float relativeSpawnFrequency = 1.0f / monsters[i].Rarity;
            edges[i] = previousEdge + relativeSpawnFrequency / totalRelativeSpawnFrequency;
            previousEdge = edges[i];
        }

        float t = Random.value;

        for (int i = 0; i < edges.Length; i++)
        {
            if (t < edges[i]) return monsters[i];
        }
        return monsters[0];
    }
}
