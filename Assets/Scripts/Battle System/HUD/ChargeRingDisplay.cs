using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChargeRingDisplay: ChargeDisplay
{
    [SerializeField] private AnimationCurve _fillCurve;
    [SerializeField] private float _chargingScale = 0.75f;
    [SerializeField] private float _drainingScale = 0.85f;
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

    [Header("Start Draining Tween")]
    [SerializeField] private float _startedDrainingTweenPeriod = 0.2f;
    [SerializeField] private TransitionType _startedDrainingTweenTransition = TransitionType.Back;
    [SerializeField] private EaseType _startedDrainingTweenEasing = EaseType.EaseOut;

    [Header("Cancel Tween")]
    [SerializeField] private float _cancelTweenPeriod = 0.3f;
    [SerializeField] private TransitionType _cancelTweenTransition = TransitionType.Back;
    [SerializeField] private EaseType _cancelTweenEasing = EaseType.EaseOut;

    private Coroutine _activeTween;
    private bool _isCharging = false;

    private SpriteRenderer Renderer {
        get {
            if (!_renderer)
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }
    private SpriteRenderer _renderer;


    protected override void SetChargeLevel(float value)
    {
        if (value >= 1 && _chargeLevel < 1)
        {
            StartScaleTween(_fullyChargedScale, _fullyChargedTweenPeriod, _fullyChargedTweenTransition, _fullyChargedTweenEasing);
        }
        else if (value <= 0)
        {
            if (_chargeLevel >= 1)
            {
                StartScaleTween(1, _throwTweenPeriod, _throwTweenTransition, _throwTweenEasing);
            }
            else if (_chargeLevel > 0)
            {
                StartScaleTween(1, _cancelTweenPeriod, _cancelTweenTransition, _cancelTweenEasing);
            }
            _isCharging = false;
        }
        else if (value > _chargeLevel && !_isCharging)
        {
            StartScaleTween(_chargingScale, _startedChargingPeriod, _startedChargingTransition, _startedChargingEasing);
            _isCharging = true;
        }
        else if (value <= _chargeLevel && _isCharging)
        {
            StartScaleTween(_drainingScale, _startedDrainingTweenPeriod, _startedDrainingTweenTransition, _startedDrainingTweenEasing);
            _isCharging = false;
        }
        
        _chargeLevel = value;
        float fillAmount = _fillCurve.Evaluate(value);
        Renderer.material.SetFloat("_FillAmount", fillAmount);
    }

    private void StartScaleTween(float finalScale, float period, TransitionType transition, EaseType easing)
    {
        if (_activeTween != null) StopCoroutine(_activeTween);
        _activeTween = StartScaleTween(transform, finalScale, period, transition, easing);
    }
}