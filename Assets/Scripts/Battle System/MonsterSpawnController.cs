using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] private MonsterData[] _monsters;
    
    private MonsterPresenter _monsterPresenter;


    void Start()
    {
        BattleEventBus.MonsterCaptured += SpawnNewMonster;
        _monsterPresenter = GetComponentInChildren<MonsterPresenter>();
        SpawnMonster();
    }

    private void OnDestroy() => BattleEventBus.MonsterCaptured -= SpawnNewMonster;


    public void SpawnMonster()
    {
        MonsterData newMonster = SelectRandom(_monsters);
        _monsterPresenter.ShowNewMonster(newMonster);
        BattleEventBus.NotifyMonsterAppeared(newMonster);
    }

    private void SpawnNewMonster(MonsterData capturedMonsterData)
    {
        List<MonsterData> monstersCopy = _monsters.ToList();
        monstersCopy.Remove(capturedMonsterData);

        MonsterData newMonster = SelectRandom(monstersCopy);
        _monsterPresenter.ShowNewMonster(newMonster);
        BattleEventBus.NotifyMonsterAppeared(newMonster);
    }

    private T SelectRandom<T>(T[] array) => array[Random.Range(0, array.Length)];
    private T SelectRandom<T>(List<T> list) => list[Random.Range(0, list.Count)];
}
