

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface ILslSampleSubscriber
{
    public abstract void ReceiveSample(LslSample sample);
}

public class SubscribableLslSampleReceiver: LslSampleReceiver
{
    [Min(0)]
    [SerializeField] private float _pollingFrequency = 0.1f;

    private List<ILslSampleSubscriber> _subscribers = new();
    private Coroutine _pollingCoroutine;
    private bool IsPolling => _pollingCoroutine != null;


    public void Subscribe(ILslSampleSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
        if (!IsPolling)
            StartPolling();
    }

    public void Unsubscribe(ILslSampleSubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
        if (_subscribers.Count == 0)
            StopPolling();
    }


    private void StartPolling()
    {
        StopPolling();
        _pollingCoroutine = StartCoroutine(RunIntermittentInletPoll());
    }

    private void StopPolling()
    {
        if (!IsPolling)
            return;

        StopCoroutine(_pollingCoroutine);
        _pollingCoroutine = null;
    }

    private IEnumerator RunIntermittentInletPoll()
    {
        while (true)
        {
            LslSample[] pulledSamples = PullAllSamples();
            foreach (LslSample sample in pulledSamples)
                NotifySubscribers(sample);

            yield return new WaitForSecondsRealtime(_pollingFrequency);
        }
    }

    private void NotifySubscribers(LslSample sample)
    {
        _subscribers.ForEach((subscriber) =>
            subscriber.ReceiveSample(sample));
    }
}