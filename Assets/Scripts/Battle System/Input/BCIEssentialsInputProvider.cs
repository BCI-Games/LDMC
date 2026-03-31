using System.Collections;
using BCIEssentials;
using BCIEssentials.Behaviours;
using BCIEssentials.LSLFramework;
using UnityEngine;

public class BCIEssentialsInputProvider : CoroutineBehaviour, IMarkerSource, IPredictionSink, IInputProvider
{
    public MarkerWriter MarkerWriter { get; set; }

    public float InputThreshold = 0.5f;

    public float InputValue => Settings.BinaryInputModeEnabled
        ? (_lastConfidenceRatio > InputThreshold ? 1 : 0)
        : Mathf.InverseLerp(InputThreshold, 1, _lastConfidenceRatio);

    private float PollingPeriod => Settings.InputPollingPeriod;
    private float EpochLength => Settings.EpochLength;

    private float _lastConfidenceRatio;


    private void Start()
    {
        MarkerWriter.PushTrialStartedMarker();
        BattleEventBus.RestPeriodStarted += InterruptIfRunning;
        BattleEventBus.RestPeriodEnded += Begin;
    }
    private void OnDestroy()
    {
        MarkerWriter.PushTrialEndsMarker();
        BattleEventBus.RestPeriodStarted -= InterruptIfRunning;
        BattleEventBus.RestPeriodEnded -= Begin;
    }


    protected override IEnumerator Run()
    {
        while (true)
        {
            MarkerWriter.PushMIClassificationMarker(2, EpochLength);
            yield return new WaitForSeconds(PollingPeriod);
        }
    }

    private void InterruptIfRunning()
    {
        if (IsRunning) Interrupt();
    }


    public void OnPrediction(Prediction prediction)
    {
        _lastConfidenceRatio = prediction.Probabilities[1];
    }


    public void SetInputThreshold(float value) => InputThreshold = value;
}