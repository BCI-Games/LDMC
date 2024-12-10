using System;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [Range(1, 5)]
    [SerializeField] protected int _sphereCount = 5;
    [SerializeField] private Vector2 _throwForce = new(15, 20);
    [SerializeField] protected float _chargePeriod = 0.5f;

    [Header("References")]
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;

    protected int _numberOfSpheresRemaining;

    protected bool IsCharging => _chargeLevel > 0;
    protected float _chargeLevel;


    protected virtual void Start()
    {
        BattleEventBus.OnBlockStarted += ResetInventory;

        _numberOfSpheresRemaining = _sphereCount;
        _chargeLevel = 0;
    }
    protected virtual void OnDestroy() => BattleEventBus.OnBlockStarted -= ResetInventory;

    protected virtual void Update()
    {
        if (IsCharging)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                if(_chargeLevel >= 1) ThrowSphere();
                else BattleEventBus.NotifyWindupCancelled();
                _chargeLevel = 0;
            }
            else
                AddFrameTimeToChargeLevel();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            AddFrameTimeToChargeLevel();
            BattleEventBus.NotifyWindupStarted();
        }
    }

    protected void AddFrameTimeToChargeLevel() => _chargeLevel += Time.deltaTime / _chargePeriod;
    

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
        _numberOfSpheresRemaining = _sphereCount;
    }
}
