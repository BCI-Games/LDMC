using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Easer: Lerper
{
    public enum TransitionType {
        Linear,
        Sine,
        Cubic,
        Expo,
        Back,
        Elastic
    }

    public static Type Transition = typeof(TransitionType);

    public enum EaseType
    {
        EaseOut,
        EaseIn,
        EaseInOut
    }


    protected Func<float, float> GetInterpolationMethod(TransitionType transition, EaseType easing)
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

    protected Func<float, float> GetEaseInMethod(Func<float, float> easeOutMethod)
        => (float t) =>  1 - easeOutMethod(1 - t);

    protected Func<float, float> GetEaseInOutMethod(Func<float, float> easeOutMethod)
        => (float t) => (t < 0.5)
            ? (1 - easeOutMethod(1 - 2 * t) / 2)
            : (1 + easeOutMethod(2 * t - 1) / 2);


    protected float EaseOutLinear(float t) => t;
    protected float EaseOutSine(float t) => Mathf.Sin(t * Mathf.PI / 2);
    protected float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);
    protected float EaseOutExpo(float t) => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    protected float EaseOutBack(float t)
    {
        const float constant = 1.70158f;

        float v1 = (constant + 1) * Mathf.Pow(t - 1, 3);
        float v2 = constant * Mathf.Pow(t - 1, 2);
        return 1 + v1 + v2;
    }
    protected float EaseOutElastic(float t)
    {
        const float constant = 2 * Mathf.PI / 3;

        return t == 0
            ? 0
            : t == 1
            ? 1
            : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * constant) + 1;
    }
}