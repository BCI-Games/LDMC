using UnityEngine;

[RequireComponent(typeof(SubscribableLslSampleReceiver))]
public class BciEssentialsInputManager: MonoBehaviour, ILslSampleSubscriber
{
    public bool IsOn;

    private SubscribableLslSampleReceiver _sampleReceiver;


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
                IsOn = (sample as LslIntegerSample).Value > 0;
                break;
            case LslPing:
                Debug.Log("Ping Received, unsubscribing");
                _sampleReceiver.Unsubscribe(this);
                break;
        }
    }
}