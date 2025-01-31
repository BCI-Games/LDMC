using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private MonsterData[] _monsters;
    
    private MonsterPresenter _monsterPresenter;
    private MonsterData _monsterExcludedFromNextSpawn;
    private bool _monsterSpawnQueued = false;


    void Start()
    {
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
        BattleEventBus.RestPeriodEnded += SpawnNewMonsterIfPending;
        _monsterPresenter = GetComponentInChildren<MonsterPresenter>();

        if (Settings.OffBockMonsterDisplayEnabled)
            SpawnMonsterFromList(_monsters);
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
    

    private void SpawnMonsterFromList(MonsterData[] monsterList)
    {
        MonsterData newMonster = SelectRandom(monsterList);
        _monsterPresenter.ShowNewMonster(newMonster);
        BattleEventBus.NotifyMonsterAppeared(newMonster);
    }

    private void SpawnNewMonster(MonsterData excludedMonster)
    {
        if (!excludedMonster)
        {
            SpawnMonsterFromList(_monsters);
            return;
        }
        List<MonsterData> monstersCopy = _monsters.ToList();
        monstersCopy.Remove(excludedMonster);
        SpawnMonsterFromList(monstersCopy.ToArray());
    }

    private T SelectRandom<T>(T[] array) => array[Random.Range(0, array.Length)];
}
