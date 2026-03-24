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

        return StartTween(
            caller,
            initialPosition, finalPosition, Vector2.LerpUnclamped,
            callbackMethod, period, transition, easing
        );
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

        return StartTween(
            caller,
            initialScale, finalScale, Vector2.LerpUnclamped,
            callbackMethod, period, transition, easing
        );
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

        return StartTween(
            caller,
            initialRotation, finalRotation, Quaternion.SlerpUnclamped,
            callbackMethod, period, transition, easing
        );
    }


    public static Coroutine StartTween<TValue>
    (
        this MonoBehaviour caller,
        TValue initialValue, TValue finalValue,
        Func<TValue, TValue, float, TValue> typedLerp,
        Action<TValue> callbackMethod, float period,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut)
    {
        Action<float> tweenMethod = BindTweenMethod(
            initialValue, finalValue, typedLerp,
            callbackMethod, transition, easing
        );
        return caller.StartCoroutine(DoTween(tweenMethod, period));
    }

    private static Action<float> BindTweenMethod<TValue>
    (
        TValue startValue, TValue finalValue,
        Func<TValue, TValue, float, TValue> typedLerp,
        Action<TValue> callbackMethod,
        TransitionType transition = TransitionType.Linear,
        EaseType easing = EaseType.EaseInOut
    )
    {
        Func<float, float> interpolationMethod = GetInterpolationMethod(transition, easing);
        return (float t) =>
        {
            float interpolatedWeight = interpolationMethod(t);
            TValue interpolatedValue = typedLerp(startValue, finalValue, interpolatedWeight);
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
}