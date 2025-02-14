using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Bci2000
{
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
            ThreadStart method, int lifetime = 500
        )
        {
            Thread workerThread = new(method);
            workerThread.Start();
            new Thread(() => {
                Thread.Sleep(lifetime);
                workerThread.Abort();
            }).Start();
        }
    }
}