using System.Collections;
using BCIEssentials;
using BCIEssentials.LSLFramework;
using UnityEngine;

public class BlockTrainBCIEssentialsTrainer: MonoBehaviour, IMarkerSource
{
    public MarkerWriter MarkerWriter { get; set; }

    private bool _isInTrial;
    private float _windowLength;

    private bool IsTraining => _markerRoutine != null;
    private Coroutine _markerRoutine = null;


    void Start()
    {
        _windowLength = GetCommonWindowLength();

        BlockTrainConductor.OffBlockStarted += SendOffBlockMarkers;
        BattleEventBus.WindupStarted += SendActiveWindowMarkers;
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    }

    void OnDestroy()
    {
        BlockTrainConductor.OffBlockStarted -= SendOffBlockMarkers;
        BattleEventBus.WindupStarted -= SendActiveWindowMarkers;
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;

        if (_isInTrial) MarkerWriter.PushTrialEndsMarker();
        if (IsTraining) StopCoroutine(_markerRoutine);
        MarkerWriter.PushTrainingCompleteMarker();
    }


    void SendOffBlockMarkers()
    {
        if (!_isInTrial)
        {
            _isInTrial = true;
            MarkerWriter.PushTrialStartedMarker();
        }

        SendMarkersForPeriod(0, Settings.OffBlockDuration);
    }

    void SendActiveWindowMarkers()
    {
        SendMarkersForPeriod(1, Settings.CharacterActiveDuration);
    }

    void OnMonsterCaptured(MonsterData _)
    {
        MarkerWriter.PushUpdateClassifierMarker();
    }


    private void SendMarkersForPeriod
    (int trainingTarget, float duration)
    => _markerRoutine = StartCoroutine
    (
        RunSendMarkersForPeriod(
            trainingTarget, duration
        )
    );
    IEnumerator RunSendMarkersForPeriod
    (int trainingTarget, float duration)
    {
        float lifetime = 0;
        while(lifetime < duration)
        {
            MarkerWriter.PushMITrainingMarker(2, trainingTarget, _windowLength);
            yield return new WaitForSeconds(_windowLength);
            lifetime += _windowLength;
        }
    }


    public static float GetCommonWindowLength()
    => GetCommonDivisor(Settings.CharacterActiveDuration, Settings.OffBlockDuration);

    public static float GetCommonDivisor(float a, float b)
    {
        if (a == b)
            return a;

        float max = Mathf.Max(a, b);
        float min = Mathf.Min(a, b);

        if (max % min == 0) return min;
        if (1 / min % max == 0) return 1 / min;
        if (1 / max % min == 0) return 1 / max;
        return 1 / (min * max);
    }
}