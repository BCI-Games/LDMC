using UnityEngine;

[RequireComponent(typeof(SubscribableLslSampleReceiver))]
public class BciEssentialsInputProvider: MonoBehaviour, ILslSampleSubscriber, IInputProvider
{
    public float InputValue => _lastPredictionWasNonZero? 1: 0;

    private SubscribableLslSampleReceiver _sampleReceiver;
    private bool _lastPredictionWasNonZero = false;


    private void Start()
    {
        _sampleReceiver = GetComponent<SubscribableLslSampleReceiver>();
        _sampleReceiver.Subscribe(this);
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