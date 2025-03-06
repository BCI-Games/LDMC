using BCIEssentials.LSLFramework;
using UnityEngine;

[RequireComponent(typeof(SubscribableLslSampleReceiver))]
public class BciEssentialsInputProvider: MonoBehaviour, ILslSampleSubscriber, IInputProvider
{
    public float InputValue => _lastPredictionWasNonZero? 1: 0;
    public float trialPeriod = 0.1f;

    private SubscribableLslSampleReceiver _sampleReceiver;
    private bool _lastPredictionWasNonZero = false;

    private float trialTimer = 0;


    private void Start()
    {
        _sampleReceiver = GetComponent<SubscribableLslSampleReceiver>();
        _sampleReceiver.Subscribe(this);

        PersistentMarkerStream.PushString("Trial Started");
    }

    private void OnDestroy()
    {
        PersistentMarkerStream.PushString("Trial Ends");
    }

    private void Update()
    {
        trialTimer += Time.deltaTime;
        if (trialTimer > trialPeriod)
        {
            trialTimer -= trialPeriod;
            PersistentMarkerStream.PushString($"mi,2,-1,{trialPeriod:f2}");
        }
    }


    public void ReceiveSample(LslSample sample)
    {
        switch (sample)
        {
            case LslIntegerSample:
                _lastPredictionWasNonZero = (sample as LslIntegerSample).Value > 0;
                break;
        }
    }
}