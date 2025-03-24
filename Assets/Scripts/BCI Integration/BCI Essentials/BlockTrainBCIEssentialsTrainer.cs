using System.Collections;
using UnityEngine;

public class BlockTrainBCIEssentialsTrainer: MonoBehaviour
{
    private bool _isInTrial;
    private float _windowLength;
    private bool _shouldUpdateClassifier;

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

        if (_isInTrial) SendTrialEndsMarker();
        if (IsTraining) StopCoroutine(_markerRoutine);
        if (_shouldUpdateClassifier) SendTrainingCompleteMarker();
    }


    void SendOffBlockMarkers()
    {
        if (!_isInTrial)
        {
            _isInTrial = true;
            SendTrialStartedMarker();
        }

        SendMarkersForPeriod(1, Settings.OffBlockDuration);
    }

    void SendActiveWindowMarkers()
    {
        SendMarkersForPeriod(2, Settings.CharacterActiveDuration);
    }

    void OnMonsterCaptured(MonsterData _)
    {
        SendUpdateClassifierMarker();
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
            SendMIEventMarker(_windowLength, trainingTarget);
            yield return new WaitForSeconds(_windowLength);
            lifetime += _windowLength;
        }
    }


    void SendTrialStartedMarker() => WriteMarker("Trial Started");
    void SendTrialEndsMarker() => WriteMarker("Trial Ends");
    void SendUpdateClassifierMarker() => WriteMarker("Update Classifier");
    void SendTrainingCompleteMarker() => WriteMarker("Training Complete");

    void SendMIEventMarker(float windowLength, int trainingTarget = -1, int optionCount = 2)
    {
        WriteMarker($"mi,{optionCount},{trainingTarget},{windowLength:f2}");
    }

    void WriteMarker(string markerString)
    {
        PersistentMarkerStream.PushString(markerString);
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