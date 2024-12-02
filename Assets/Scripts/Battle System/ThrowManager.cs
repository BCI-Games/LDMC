using System;
using System.Collections;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int _sphereCount = 5;
    [SerializeField] private Vector2 _throwForce = new(15, 20);

    [Header("References")]
    [SerializeField] private SphereInventoryDisplay _inventoryDisplay;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;

    protected int _numberOfSpheresRemaining;


    protected virtual void Start()
    {
        BattleEventBus.PlayerTurnStarted += ResetInventory;
        BattleEventBus.OpponentTurnStarted += ResetInventory;

        _numberOfSpheresRemaining = _sphereCount;
        _inventoryDisplay.BuildSphereIcons(_sphereCount);
    }
    protected virtual void OnDestroy()
    {
        BattleEventBus.PlayerTurnStarted -= ResetInventory;
        BattleEventBus.OpponentTurnStarted -= ResetInventory;
    }

    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ThrowSphere();
    }
    

    public void ThrowSphere()
    {
        if(_numberOfSpheresRemaining > 0)
        {
            _numberOfSpheresRemaining--;
            GameObject sphere = Instantiate(_spherePrefab, _spawnLocation);
            sphere.GetComponent<Rigidbody2D>().AddForce(_throwForce, ForceMode2D.Impulse);

            BattleEventBus.NotifySphereThrown();
            if (_numberOfSpheresRemaining == 0)
                BattleEventBus.NotifyLastSphereThrown();
        }
        else
        {
            Debug.Log("No more spheres left");
        }
    }

    public void ResetInventory()
    {
        _inventoryDisplay.ResetIcons();
        _numberOfSpheresRemaining = _sphereCount;
    }
}
