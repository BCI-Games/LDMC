

using System.Collections.Generic;
using UnityEngine;

public interface ILslSampleSubscriber
{
    public abstract void ReceiveSample(LslSample sample);
}

public class SubscribableLslSampleReceiver: LslSampleReceiver
{
    [Min(0)]
    [SerializeField] private float _pollingPeriod = 0.1f;

    private List<ILslSampleSubscriber> _subscribers = new();
    private Queue<ILslSampleSubscriber> _unsubscribeQueue = new();

    private bool _isPolling = false;
    private bool _isNotifyingSubscribers = false;
    private float _pollTimer;


    private void Update()
    {
        if (!_isPolling) return;

        _pollTimer += Time.deltaTime;
        if (_pollTimer >  _pollingPeriod)
        {
            _pollTimer -= _pollingPeriod;

            foreach (LslSample sample in PullAllSamples())
                NotifySubscribers(sample);
            PruneSubscriberList();
        }
    }


    public void Subscribe(ILslSampleSubscriber subscriber)
    {
        _subscribers.Add(subscriber);
        if (!_isPolling)
            StartPolling();
    }

    public void Unsubscribe(ILslSampleSubscriber subscriber)
    {
        if (_isNotifyingSubscribers)
            _unsubscribeQueue.Enqueue(subscriber);
        else
            _subscribers.Remove(subscriber);

        if (_subscribers.Count - _unsubscribeQueue.Count == 0)
            StopPolling();
    }


    private void StartPolling()
    {
        if (!HasLiveInlet && !IsResolvingStream)
            FindAndConnectToStream();
        
        _isPolling = true;
        _pollTimer = 0;
    }

    private void StopPolling()
    {
        if (HasLiveInlet)
            CloseInlet();
        
        _isPolling = false;
        _pollTimer = 0;
    }


    private void NotifySubscribers(LslSample sample)
    {
        _isNotifyingSubscribers = true;
        foreach(var subscriber in _subscribers)
        {
            if (!_unsubscribeQueue.Contains(subscriber))
                subscriber.ReceiveSample(sample);
        }
        _isNotifyingSubscribers = false;
    }

    private void PruneSubscriberList()
    {
        ILslSampleSubscriber unsubscriber;
        while (_unsubscribeQueue.TryDequeue(out unsubscriber))
        {
            _subscribers.Remove(unsubscriber);
        }
        foreach (var s in _unsubscribeQueue)
            _subscribers.Remove(s);
    }
}