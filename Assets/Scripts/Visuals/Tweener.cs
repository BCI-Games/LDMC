using System;
using System.Collections;
using UnityEngine;

public abstract class Tweener: Easer
{
    protected Coroutine StartPositionTween(Transform target, Vector2 finalPosition, float period,
        TransitionType transition = TransitionType.Linear, EaseType easing = EaseType.EaseInOut)
    {
        Vector2 initialPosition = target.localPosition;
        Action<Vector2> callbackMethod = (Vector2 tweenedPosition) => target.localPosition = tweenedPosition;

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
}