using UnityEngine;

public class ChargeRingDisplay: Tweener
{
    [SerializeField] private AnimationCurve _fillCurve;
    [SerializeField] private float _chargingScale = 0.75f;
    [SerializeField] private float _fullyChargedScale = 0.5f;

    [Header("Start Charging Tween")]
    [SerializeField] private float _startedChargingPeriod = 0.2f;
    [SerializeField] private TransitionType _startedChargingTransition = TransitionType.Back;
    [SerializeField] private EaseType _startedChargingEasing = EaseType.EaseOut;

    [Header("Finish Charging Tween")]
    [SerializeField] private float _fullyChargedTweenPeriod = 0.2f;
    [SerializeField] private TransitionType _fullyChargedTweenTransition = TransitionType.Back;
    [SerializeField] private EaseType _fullyChargedTweenEasing = EaseType.EaseOut;

    [Header("Throw Tween")]
    [SerializeField] private float _throwTweenPeriod = 0.6f;
    [SerializeField] private TransitionType _throwTweenTransition = TransitionType.Elastic;
    [SerializeField] private EaseType _throwTweenEasing = EaseType.EaseOut;

    [Header("Cancel Tween")]
    [SerializeField] private float _cancelTweenPeriod = 0.4f;
    [SerializeField] private TransitionType _cancelTweenTransition = TransitionType.Cubic;
    [SerializeField] private EaseType _cancelTweenEasing = EaseType.EaseInOut;

    private Coroutine _activeTween;


    public float ChargeLevel{
        get => _chargeLevel;
        set => SetChargeLevel(value);
    }
    protected float _chargeLevel;

    private SpriteRenderer _renderer;


    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }


    private void SetChargeLevel(float value)
    {
        if (value >= 1 && _chargeLevel < 1)
        {
            StartScaleTween(_fullyChargedScale, _fullyChargedTweenPeriod, _fullyChargedTweenTransition, _fullyChargedTweenEasing);
        }
        else if (value > 0 && _chargeLevel <= 0)
        {
            StartScaleTween(_chargingScale, _startedChargingPeriod, _startedChargingTransition, _startedChargingEasing);
        }
        else if (value == 0)
        {
            if (_chargeLevel >= 1)
                StartScaleTween(1, _throwTweenPeriod, _throwTweenTransition, _throwTweenEasing);
            else
                StartScaleTween(1, _cancelTweenPeriod, _cancelTweenTransition, _cancelTweenEasing);
        }
        
        _chargeLevel = value;
        float fillAmount = _fillCurve.Evaluate(value);
        _renderer.material.SetFloat("_FillAmount", fillAmount);
    }

    private void StartScaleTween(float finalScale, float period, TransitionType transition, EaseType easing)
    {
        if (_activeTween != null) StopCoroutine(_activeTween);
        _activeTween = StartScaleTween(transform, finalScale, period, transition, easing);
    }
}