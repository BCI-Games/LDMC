using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public static class MethodExtensions
{
    public static void ExecuteWhen
    (
        this MonoBehaviour behaviour,
        Func<bool> predicate, Action callback
    )
    => behaviour.StartCoroutine(
        RunExecuteWhen(predicate, callback)
    );

    public static IEnumerator RunExecuteWhen
    (
        Func<bool> predicate, Action callback
    )
    {
        while (!predicate())
        {
            yield return new WaitForEndOfFrame();
        }
        callback();
    }


    public static void ExecuteMethodInThread
    (
        this MonoBehaviour b, ThreadStart method
    )
    => new Thread(method).Start();
}