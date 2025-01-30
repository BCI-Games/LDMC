using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    [SerializeField] private Vector2 _throwForce = new(15, 20);

    [Header("References")]
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private GameObject _spherePrefab;

    protected bool SpheresRemain => _numberOfSpheresRemaining > 0;
    private int _sphereCount;
    private int _numberOfSpheresRemaining;
    private float _chargePeriod;


    protected bool ShouldCharge {get => GetShouldCharge();}
    protected bool IsCharging => _chargeLevel > 0;
    protected bool ChargeThresholdIsMet => _chargeLevel >= 1;
    private float _chargeLevel = 0;


    protected virtual void Start()
    {
        Settings.AddAndInvokeModificationCallback(UpdateParametersFromSettings);
        BattleEventBus.RestPeriodEnded += ResetInventory;
    }
    protected virtual void OnDestroy()
    {
        Settings.Modified -= UpdateParametersFromSettings;
        BattleEventBus.RestPeriodEnded -= ResetInventory;
    }

    protected virtual void UpdateParametersFromSettings()
    {
        _chargePeriod = Settings.CharacterActiveDuration;
        _sphereCount = Settings.OnBlockCycleCount;
    }

    private void Update()
    {
        if (ShouldCharge && SpheresRemain)
        {
            if (!IsCharging)
                BattleEventBus.NotifyWindupStarted();
            AddFrameTimeToChargeLevel();
        }
        else if (IsCharging)
        {
            if (ChargeThresholdIsMet) ThrowSphere();
            else BattleEventBus.NotifyWindupCancelled();
            ResetChargeLevel();
        }
    }
    

    public void ThrowSphere()
    {
        _numberOfSpheresRemaining--;
        GameObject sphere = Instantiate(_spherePrefab, _spawnLocation);
        sphere.GetComponent<Rigidbody2D>().AddForce(_throwForce, ForceMode2D.Impulse);

        BattleEventBus.NotifySphereThrown();
        if (!SpheresRemain)
            BattleEventBus.NotifyLastSphereThrown();
    }

    protected void ResetInventory() => _numberOfSpheresRemaining = _sphereCount;

    protected void AddFrameTimeToChargeLevel() => _chargeLevel += Time.deltaTime / _chargePeriod;
    protected void ResetChargeLevel() => _chargeLevel = 0;
    protected virtual bool GetShouldCharge() => Input.GetKey(KeyCode.Space);
}
