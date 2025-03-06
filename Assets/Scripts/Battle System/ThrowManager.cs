using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    public enum DrainMode {Immediate, Gradual, None}

    [SerializeField] private Vector2 _throwForce = new(15, 20);
    [SerializeField] private DrainMode _drainMode;

    [Header("References")]
    [SerializeField] private GameObject _spherePrefab;
    [SerializeField] private ChargeDisplay _chargeDisplay;

    protected bool SpheresRemain => _numberOfSpheresRemaining > 0;
    private int _sphereCount;
    private int _numberOfSpheresRemaining;

    protected float ChargePeriod;

    protected bool ShouldCharge {get => GetShouldCharge();}
    protected bool IsCharging => ChargeLevel > 0;
    protected bool ChargeThresholdIsMet => ChargeLevel >= 1;
    protected float ChargeLevel {
        get => _chargeLevel;
        set {
            _chargeLevel = value;
            if (_chargeDisplay) _chargeDisplay.ChargeLevel = value;
        }
    }
    private float _chargeLevel = 0;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.8f, 0.8f);
        Gizmos.DrawSphere(transform.position, 0.4f);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, _throwForce / 8f);
    }

    protected virtual void Start()
    {
        Settings.AddAndInvokeModificationCallback(UpdateParametersFromSettings);
        BattleEventBus.RestPeriodEnded += ResetInventory;
        ResetChargeLevel();
    }
    protected virtual void OnDestroy()
    {
        Settings.Modified -= UpdateParametersFromSettings;
        BattleEventBus.RestPeriodEnded -= ResetInventory;
    }

    protected virtual void UpdateParametersFromSettings()
    {
        ChargePeriod = Settings.CharacterActiveDuration;
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
            if (ChargeThresholdIsMet)
            {
                ThrowSphere();
                ResetChargeLevel();
            }
            
            DrainCharge();
            
            if (!IsCharging)
                BattleEventBus.NotifyWindupCancelled();
        }
    }
    

    public void ThrowSphere()
    {
        _numberOfSpheresRemaining--;
        GameObject sphere = Instantiate(_spherePrefab, transform);
        sphere.GetComponent<Rigidbody2D>().AddForce(_throwForce, ForceMode2D.Impulse);

        BattleEventBus.NotifySphereThrown();
        if (!SpheresRemain)
            BattleEventBus.NotifyLastSphereThrown();
    }

    private void ResetInventory() => _numberOfSpheresRemaining = _sphereCount;

    protected virtual void AddFrameTimeToChargeLevel()
    => ChargeLevel += Time.deltaTime / ChargePeriod;
    private void DrainCharge()
    {
        switch(_drainMode)
        {
            case DrainMode.Immediate:
                ResetChargeLevel();
                break;
            case DrainMode.Gradual:
                ChargeLevel -= Time.deltaTime / ChargePeriod;
                break;
            case DrainMode.None:
                ChargeLevel = ChargeLevel;
                break;
        }
    }
    
    protected void ResetChargeLevel() => ChargeLevel = 0;
    protected virtual bool GetShouldCharge() => Input.GetKey(KeyCode.Space);
}
