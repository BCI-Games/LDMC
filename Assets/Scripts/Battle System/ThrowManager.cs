using System;
using System.Collections;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int _sphereCount = 5;
    [SerializeField] private Vector2 _throwForce = new(15, 20);
    [SerializeField] private float _autoThrowDelay = 1;

    [Header("References")]
    [SerializeField] private SphereInventoryDisplay _inventoryDisplay;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;

    private int _numberOfSpheresRemaining;

    private bool _isAutoThrowing;
    private Coroutine _autoThrowCoroutine;


    private void Start()
    {
        BattleEventBus.MonsterAppeared += ResetInventory;
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;

        _numberOfSpheresRemaining = _sphereCount;
        _inventoryDisplay.BuildSphereIcons(_sphereCount);
    }
    private void OnDestroy()
    {
        BattleEventBus.MonsterAppeared -= ResetInventory;
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ThrowSphere();

        if(Input.GetKeyDown(KeyCode.R))
            ResetInventory();

        if(Input.GetKeyDown(KeyCode.A))
            StartAutoThrow();
    }
    

    public void ThrowSphere()
    {
        if(_numberOfSpheresRemaining > 0)
        {
            _numberOfSpheresRemaining--;
            GameObject sphere = Instantiate(_spherePrefab, _spawnLocation);
            sphere.GetComponent<Rigidbody2D>().AddForce(_throwForce, ForceMode2D.Impulse);
            BattleEventBus.NotifySphereThrown();
        }
        else
        {
            Debug.Log("No more spheres left");
        }
    }


    public void ResetInventory(MonsterData monsterData) => ResetInventory();
    public void ResetInventory()
    {
        _inventoryDisplay.ResetIcons();
        _numberOfSpheresRemaining = _sphereCount;
    }

    public void StartAutoThrow()
    {
        StopAutoThrow();
        _autoThrowCoroutine = StartCoroutine(RunAutoThrow());
    }

    private void OnMonsterCaptured(MonsterData monsterData) => StopAutoThrow();
    public void StopAutoThrow()
    {
        if (_isAutoThrowing) StopCoroutine(_autoThrowCoroutine);
    }

    private IEnumerator RunAutoThrow()
    {
        _isAutoThrowing = true;
        while(_numberOfSpheresRemaining > 0)
        {
            ThrowSphere();
            yield return new WaitForSeconds(_autoThrowDelay);
        }
        _isAutoThrowing = false;
    }
}
