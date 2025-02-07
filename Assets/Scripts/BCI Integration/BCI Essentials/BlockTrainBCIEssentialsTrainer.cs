using UnityEngine;
using BCIEssentials.LSLFramework;

[RequireComponent(typeof(LSLMarkerStream))]
public class BlockTrainBCIEssentialsTrainer: MonoBehaviour
{
    private LSLMarkerStream _markerOutlet;
    private bool isInTrial;

    void Start()
    {
        _markerOutlet = GetComponent<LSLMarkerStream>();

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


    void SendTrialStartedMarker() => _markerOutlet.Write("Trial Started");
    void SendTrialEndsMarker() => _markerOutlet.Write("Trial Ends");
    void SendUpdateClassifierMarker() => _markerOutlet.Write("Update Classifier");

    void SendMIEventMarker(float windowLength, int trainingTarget = -1, int optionCount = 2)
    {
        _markerOutlet.Write($"mi,{optionCount},{trainingTarget},{windowLength:f2}");
    }
}