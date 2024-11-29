using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITweener: MonoBehaviour
{
    public enum TransitionType {
        Linear,
        Sine,
        Cubic,
        Expo,
        Back,
        Elastic
    }

    public enum EaseType
    {
        EaseOut,
        EaseIn,
        EaseInOut
    }


    protected Coroutine StartPositionTween(Transform target, Vector2 finalPosition, float period,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Vector2 initialPosition = target.position;
        Action<Vector2> callbackMethod = (Vector2 tweenedPosition) => target.position = tweenedPosition;

        return StartTween(initialPosition, finalPosition, callbackMethod, period, transition, easing);
    }

    protected Coroutine StartScaleTween(Transform target, Vector2 finalScale, float period,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Vector2 initialScale = target.localScale;
        Action<Vector2> callbackMethod = (Vector2 tweenedScale) => target.localScale = tweenedScale;

        return StartTween(initialScale, finalScale, callbackMethod, period, transition, easing);
    }

    protected Coroutine StartRotationTween(Transform target, Quaternion finalRotation, float period,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Quaternion initialRotation = target.localRotation;
        Action<Quaternion> callbackMethod = (Quaternion tweenedRotation) => target.localRotation = tweenedRotation;

        return StartTween(initialRotation, finalRotation, callbackMethod, period, transition, easing);
    }


    protected Coroutine StartTween<TValue>(TValue initialValue, TValue finalValue,
        Action<TValue> callbackMethod, float period,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Action<float> tweenMethod = BindTweenMethod(initialValue, finalValue,callbackMethod, transition, easing);
        return StartCoroutine(DoTween(tweenMethod, period));
    }

    private Action<float> BindTweenMethod<TValue>(TValue startValue, TValue finalValue, Action<TValue> callbackMethod,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Func<float, float> interpolationMethod = GetInterpolationMethod(transition, easing);
        return (float t) => {
            float interpolatedWeight = interpolationMethod(t);
            TValue interpolatedValue = Lerp((dynamic)startValue, (dynamic)finalValue, interpolatedWeight);
            callbackMethod(interpolatedValue);
        };
    }

    private IEnumerator DoTween(Action<float> tweenMethod, float period)
    {
        float timer = 0;

        while (timer < period)
        {
            float t = timer / period;

            tweenMethod(t);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


    protected TValue Lerp<TValue>(TValue a, TValue b, float t)
    {
        Debug.LogWarning("Lerp Method not defined for type " + typeof(TValue));
        return a;
    }
    private float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);
    private Color Lerp(Color a, Color b, float t) => Color.Lerp(a, b, t);
    private Vector2 Lerp(Vector2 a, Vector2 b, float t) => Vector2.Lerp(a, b, t);
    private Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
    private Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.Lerp(a, b, t);


    private Func<float, float> GetInterpolationMethod(TransitionType transition, EaseType easing)
    {
        Dictionary<TransitionType, Func<float, float>> transitionMethods = new()
        {
            {TransitionType.Linear, EaseOutLinear},
            {TransitionType.Sine, EaseOutSine},
            {TransitionType.Cubic, EaseOutCubic},
            {TransitionType.Expo, EaseOutExpo},
            {TransitionType.Back, EaseOutBack},
            {TransitionType.Elastic, EaseOutElastic},
        };

        Func<float, float> interpolationMethod = transitionMethods[transition];

        switch(easing)
        {
            case EaseType.EaseIn: return GetEaseInMethod(interpolationMethod);
            case EaseType.EaseInOut: return GetEaseInOutMethod(interpolationMethod);
            default: return interpolationMethod;
        }
    }

    private Func<float, float> GetEaseInMethod(Func<float, float> easeOutMethod)
        => (float t) =>  1 - easeOutMethod(1 - t);

    private Func<float, float> GetEaseInOutMethod(Func<float, float> easeOutMethod)
        => (float t) => (t < 0.5)
            ? (1 - easeOutMethod(1 - 2 * t) / 2)
            : (1 + easeOutMethod(2 * t - 1) / 2);


    private float EaseOutLinear(float t) => t;
    private float EaseOutSine(float t) => Mathf.Sin(t * Mathf.PI / 2);
    private float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);
    private float EaseOutExpo(float t) => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    private float EaseOutBack(float t)
    {
        const float constant = 1.70158f;

        float v1 = (constant + 1) * Mathf.Pow(t - 1, 3);
        float v2 = constant * Mathf.Pow(t - 1, 2);
        return 1 + v1 + v2;
    }
    private float EaseOutElastic(float t)
    {
        const float constant = 2 * Mathf.PI / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * constant) + 1;
    }
}