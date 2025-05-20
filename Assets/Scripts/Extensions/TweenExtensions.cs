using System;
using System.Collections;
using UnityEngine;

using static Easings;

public static class Tweener
{
    public static Coroutine StartPositionTween
    (
        this MonoBehaviour caller, Vector2 finalPosition, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Transform target = caller.transform;
        Vector2 initialPosition = target.localPosition;
        void callbackMethod(Vector2 tweenedPosition) => target.localPosition = tweenedPosition;

        return StartTween(caller, initialPosition, finalPosition, callbackMethod, period, transition, easing);
    }

    public static Coroutine StartScaleTween
    (
        this MonoBehaviour caller, float finalScale, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Vector2 finalScaleVector = Vector2.one * finalScale;

        return StartScaleTween(caller, finalScaleVector, period, transition, easing);
    }
    public static Coroutine StartScaleTween
    (
        this MonoBehaviour caller, Vector2 finalScale, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Transform target = caller.transform;
        Vector2 initialScale = target.localScale;
        void callbackMethod(Vector2 tweenedScale) => target.localScale = tweenedScale;

        return StartTween(caller, initialScale, finalScale, callbackMethod, period, transition, easing);
    }

    public static Coroutine StartRotationTween
    (
        this MonoBehaviour caller, float finalRotation, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Quaternion finalRotationQuaternion = Quaternion.Euler(new(0, 0, finalRotation));

        return StartRotationTween(caller, finalRotationQuaternion, period, transition, easing);
    }
    public static Coroutine StartRotationTween
    (
        this MonoBehaviour caller, Quaternion finalRotation, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Transform target = caller.transform;
        Quaternion initialRotation = target.localRotation;
        void callbackMethod(Quaternion tweenedRotation) => target.localRotation = tweenedRotation;

        return StartTween(caller, initialRotation, finalRotation, callbackMethod, period, transition, easing);
    }


    public static Coroutine StartTween<TValue>
    (
        this MonoBehaviour caller,
        TValue initialValue, TValue finalValue,
        Action<TValue> callbackMethod, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut)
    {
        Action<float> tweenMethod = BindTweenMethod(initialValue, finalValue,callbackMethod, transition, easing);
        return caller.StartCoroutine(DoTween(tweenMethod, period));
    }

    private static Action<float> BindTweenMethod<TValue>
    (
        TValue startValue, TValue finalValue,
        Action<TValue> callbackMethod,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Func<float, float> interpolationMethod = GetInterpolationMethod(transition, easing);
        return (float t) => {
            float interpolatedWeight = interpolationMethod(t);
            TValue interpolatedValue = Lerp((dynamic)startValue, (dynamic)finalValue, interpolatedWeight);
            callbackMethod(interpolatedValue);
        };
    }

    private static IEnumerator DoTween
    (
        Action<float> tweenMethod, float period
    )
    {
        float timer = 0;

        while (timer < period)
        {
            float t = timer / period;

            tweenMethod(t);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        tweenMethod(1);
    }


    public static TValue Lerp<TValue>(TValue a, TValue b, float t) where TValue: struct
    {
        Debug.LogWarning("Lerp Method not defined for type " + typeof(TValue));
        return a;
    }
    public static float Lerp(float a, float b, float t) => Mathf.LerpUnclamped(a, b, t);
    public static Color Lerp(Color a, Color b, float t) => Color.LerpUnclamped(a, b, t);
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => Vector2.LerpUnclamped(a, b, t);
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.LerpUnclamped(a, b, t);
    public static Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.LerpUnclamped(a, b, t);
}