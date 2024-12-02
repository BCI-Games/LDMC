using UnityEngine;

public class ChargeRingDisplay: Tweener
{
    [SerializeField] private AnimationCurve _fillCurve;
    [SerializeField] private float _chargingScale = 0.75f;

    [Header("Start Charging Tween")]
    [SerializeField] private float _chargeTweenPeriod = 0.2f;
    [SerializeField] private TransitionType _chargeTweenTransition = TransitionType.Back;
    [SerializeField] private EaseType _chargeTweenEasing = EaseType.EaseOut;

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
        if (value > 0 && _chargeLevel <= 0)
        {
            StartScaleTween(_chargingScale, _chargeTweenPeriod, _chargeTweenTransition, _chargeTweenEasing);
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