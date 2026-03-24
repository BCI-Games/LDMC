using BCIEssentials;
using BCIEssentials.LSLFramework;
using UnityEngine;

public class BCIEssentialsInputProvider: MonoBehaviour, IMarkerSource, IPredictionSink, IInputProvider
{
    public MarkerWriter MarkerWriter { get; set; }

    public float InputValue => _lastConfidenceRatio;
    private float PollingPeriod => Settings.InputPollingPeriod;
    private float EpochLength => Settings.EpochLength;

    private float _lastConfidenceRatio;
    private float _pollingTimer = 0;


    private void Start() => MarkerWriter.PushTrialStartedMarker();
    private void OnDestroy() => MarkerWriter.PushTrialEndsMarker();


    private void Update()
    {
        _pollingTimer += Time.deltaTime;
        if (_pollingTimer > PollingPeriod)
        {
            _pollingTimer -= PollingPeriod;
            MarkerWriter.PushMIClassificationMarker(2, EpochLength);
        }
    }


    public void OnPrediction(Prediction prediction)
    {
        _lastConfidenceRatio = prediction.Probabilities[1];
    }
}