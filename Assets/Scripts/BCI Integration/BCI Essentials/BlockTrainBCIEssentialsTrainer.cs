using System.Collections;
using BCIEssentials;
using BCIEssentials.LSLFramework;
using UnityEngine;

public class BlockTrainBCIEssentialsTrainer : MonoBehaviour, IMarkerSource
{
    public MarkerWriter MarkerWriter { get; set; }

    private Coroutine _markerRoutine = null;
    private bool _isInTrial;


    void Start()
    {
        BlockTrainConductor.OffBlockStarted += SendOffBlockMarkers;
        BattleEventBus.WindupStarted += SendActiveMarkers;
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    }

    void OnDestroy()
    {
        BlockTrainConductor.OffBlockStarted -= SendOffBlockMarkers;
        BattleEventBus.WindupStarted -= SendActiveMarkers;
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;

        if (_isInTrial) MarkerWriter.PushTrialEndsMarker();
        if (_markerRoutine != null) StopCoroutine(_markerRoutine);
        MarkerWriter.PushTrainingCompleteMarker();
    }


    void SendOffBlockMarkers()
    {
        if (!_isInTrial)
        {
            _isInTrial = true;
            MarkerWriter.PushTrialStartedMarker();
        }

        SendMarkers(0, Settings.MinimumSharedEpochCount);
    }
    void SendActiveMarkers()
    {
        SendMarkers(1, Settings.MinimumSharedEpochCount);
    }

    void OnMonsterCaptured(MonsterData _)
    {
        MarkerWriter.PushTrialEndsMarker();
        MarkerWriter.PushUpdateClassifierMarker();
    }


    private void SendMarkers(int trainingTarget, int count)
    => _markerRoutine = StartCoroutine
    (
        RunSendMarkers(trainingTarget, count)
    );
    IEnumerator RunSendMarkers(int trainingTarget, int count)
    {
        WaitForSeconds trialSegmentDuration = new(Settings.EpochLength);

        for (int i = 0; i < count; i++)
        {
            MarkerWriter.PushMITrainingMarker(2, trainingTarget, Settings.EpochLength);
            yield return trialSegmentDuration;
        }
        _markerRoutine = null;
    }
}