using UnityEngine;
using static Easings;

[RequireComponent(typeof(SpriteRenderer))]
public class ChargeRingDisplay: ChargeDisplay
{
    [SerializeField] private AnimationCurve _fillCurve;

    [SerializeField] private ScaleTweenParameters _startedChargingTween = new(0.75f);
    [SerializeField] private ScaleTweenParameters _fullyChargedTween = new(0.5f);
    [SerializeField] private ScaleTweenParameters _throwTween = new(1, 0.6f, TransitionType.Elastic);
    [SerializeField] private ScaleTweenParameters _startedDrainingTween = new(0.85f);
    [SerializeField] private ScaleTweenParameters _cancelTween = new(1, 0.3f);

    [SerializeField] private ScaleTweenParameters _showTween = new(1, 0.5f);
    [SerializeField] private ScaleTweenParameters _hideTween = new(0, 0.4f, TransitionType.Cubic);

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


    protected override void Show() => RestartScaleTween(_showTween);
    protected override void Hide() => RestartScaleTween(_hideTween);


    protected override void SetChargeLevel(float value)
    {
        if (value >= 1 && _chargeLevel < 1) RestartScaleTween(_fullyChargedTween);
        else if (value <= 0)
        {
            if (_chargeLevel >= 1) RestartScaleTween(_throwTween);
            else if (_chargeLevel > 0) RestartScaleTween(_cancelTween);
            _isCharging = false;
        }
        else if (value > _chargeLevel && !_isCharging)
        {
            RestartScaleTween(_startedChargingTween);
            _isCharging = true;
        }
        else if (value <= _chargeLevel && _isCharging)
        {
            RestartScaleTween(_startedDrainingTween);
            _isCharging = false;
        }
        
        _chargeLevel = value;
        float fillAmount = _fillCurve.Evaluate(value);
        Renderer.material.SetFloat("_FillAmount", fillAmount);
    }

    private void RestartScaleTween(ScaleTweenParameters tweenParameters)
    => RestartScaleTween(
        tweenParameters.Scale, tweenParameters.Period,
        tweenParameters.Transition, tweenParameters.Easing
    );
    private void RestartScaleTween(float finalScale, float period, TransitionType transition, EaseType easing)
    {
        if (_activeTween != null) StopCoroutine(_activeTween);
        _activeTween = this.StartScaleTween(finalScale, period, transition, easing);
    }


    [System.Serializable]
    public struct ScaleTweenParameters
    {
        public float Scale;
        public float Period;
        public TransitionType Transition;
        public EaseType Easing;

        public ScaleTweenParameters(
            float scale = 1, float period = 0.2f,
            TransitionType transition = TransitionType.Back,
            EaseType easing = EaseType.EaseOut
        )
        {
            Scale = scale;
            Period = period;
            Transition = transition;
            Easing = easing;
        }
    }
}