using UnityEngine;

public class BlockTrainBCIEssentialsTrainer: MonoBehaviour
{
    private bool isInTrial;

    void Start()
    {
        BlockTrainConductor.OffBlockStarted += SendOffTrialMarkers;
        BattleEventBus.WindupStarted += SendOnTrialMarkers;
        BattleEventBus.SphereThrown += EndPreviousTrial;
    }

    void OnDestroy()
    {
        BlockTrainConductor.OffBlockStarted -= SendOffTrialMarkers;
        BattleEventBus.WindupStarted -= SendOnTrialMarkers;
        BattleEventBus.SphereThrown -= EndPreviousTrial;

        EndPreviousTrial();
    }

    
    void SendOffTrialMarkers()
    {
        EndPreviousTrial();
        SendTrialStartedMarker();
        SendMIEventMarker(Settings.OffBlockDuration, 0);
        isInTrial = true;
    }


    void SendOnTrialMarkers()
    {
        EndPreviousTrial();
        SendTrialStartedMarker();
        SendMIEventMarker(Settings.CharacterActiveDuration, 1);
        isInTrial = true;
    }

    void EndPreviousTrial()
    {
        if (!isInTrial) return;
        SendTrialEndsMarker();
        SendUpdateClassifierMarker();
        isInTrial = false;
    }


    void SendTrialStartedMarker() => WriteMarker("Trial Started");
    void SendTrialEndsMarker() => WriteMarker("Trial Ends");
    void SendUpdateClassifierMarker() => WriteMarker("Update Classifier");

    void SendMIEventMarker(float windowLength, int trainingTarget = -1, int optionCount = 2)
    {
        WriteMarker($"mi,{optionCount},{trainingTarget},{windowLength:f2}");
    }

    void WriteMarker(string markerString)
    {
        PersistentMarkerStream.PushString(markerString);
    }
}