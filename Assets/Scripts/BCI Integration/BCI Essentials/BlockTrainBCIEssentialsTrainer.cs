using UnityEngine;

public class BlockTrainBCIEssentialsTrainer: MonoBehaviour
{
    private bool _isInTrial;
    private bool _hasTrained;

    void Start()
    {
        BlockTrainConductor.OffBlockStarted += SendOffBlockMarker;
        BattleEventBus.WindupStarted += SendOnBlockMarker;
        BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    }

    void OnDestroy()
    {
        BlockTrainConductor.OffBlockStarted -= SendOffBlockMarker;
        BattleEventBus.WindupStarted -= SendOnBlockMarker;
        BattleEventBus.MonsterCaptured -= OnMonsterCaptured;

        if (_isInTrial) SendTrialEndsMarker();
        if (_hasTrained) SendTrainingCompleteMarker();
    }


    void SendOffBlockMarker()
    {
        if (!_isInTrial)
        {
            _isInTrial = true;
            SendTrialStartedMarker();
        }
        SendMIEventMarker(Settings.OffBlockDuration, 0);
    }

    void SendOnBlockMarker()
    {
        SendMIEventMarker(Settings.CharacterActiveDuration, 1);
    }

    void OnMonsterCaptured(MonsterData _)
    {
        _hasTrained = true;
        SendUpdateClassifierMarker();
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
}