using System;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] private int _sphereCount = 5;
    [SerializeField] private Vector2 _throwForce = new(15, 20);
    [SerializeField] protected float _chargePeriod = 0.5f;

    [Header("References")]
    [SerializeField] private SphereInventoryDisplay _inventoryDisplay;
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;
    [SerializeField] private ChargeRingDisplay _chargeDisplay;

    protected int _numberOfSpheresRemaining;

    protected bool IsCharging => _chargeLevel > 0;
    protected float ChargeLevel {
        get => _chargeLevel;
        set {
            _chargeLevel = value;
            if (_chargeDisplay) _chargeDisplay.ChargeLevel = _chargeLevel;
        }
    }
    private float _chargeLevel;


    protected virtual void Start()
    {
        BattleEventBus.PlayerTurnStarted += ResetInventory;
        BattleEventBus.OpponentTurnStarted += ResetInventory;

        _numberOfSpheresRemaining = _sphereCount;
        _inventoryDisplay.BuildSphereIcons(_sphereCount);
        ChargeLevel = 0;
    }
    protected virtual void OnDestroy()
    {
        BattleEventBus.PlayerTurnStarted -= ResetInventory;
        BattleEventBus.OpponentTurnStarted -= ResetInventory;
    }

    protected virtual void Update()
    {
        if (IsCharging)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                if(ChargeLevel >= 1)
                    ThrowSphere();
                ChargeLevel = 0;
            }
            else
                AddFrameTimeToChargeLevel();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
            AddFrameTimeToChargeLevel();
    }

    protected void AddFrameTimeToChargeLevel() => ChargeLevel += Time.deltaTime / _chargePeriod;
    

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
