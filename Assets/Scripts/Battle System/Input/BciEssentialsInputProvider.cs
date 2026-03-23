using BCIEssentials;
using BCIEssentials.LSLFramework;
using UnityEngine;

public class BciEssentialsInputProvider: MonoBehaviour, IMarkerSource, IPredictionSink, IInputProvider
{
    public MarkerWriter MarkerWriter { get; set; }

    public float InputValue => _lastConfidenceRatio;
    public float EpochLength = 1.0f;

    private float _lastConfidenceRatio;
    private float _epochTimer = 0;


    private void Start() => MarkerWriter.PushTrialStartedMarker();
    private void OnDestroy() => MarkerWriter.PushTrialEndsMarker();


    private void Update()
    {
        _epochTimer += Time.deltaTime;
        if (_epochTimer > EpochLength)
        {
            _epochTimer -= EpochLength;
            MarkerWriter.PushMIClassificationMarker(2, EpochLength);
        }
    }


    public void OnPrediction(Prediction prediction)
    {
        _lastConfidenceRatio = prediction.Probabilities[1];
    }
}