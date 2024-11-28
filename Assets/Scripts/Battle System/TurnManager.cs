using UnityEngine;

public class TurnManager: MonoBehaviour
{
    public static bool IsPlayerTurn = true;


    private void Start()
    {
        BattleEventBus.LastSphereThrown += StartNextTurn;
    }

    private void OnDestroy()
    {
        BattleEventBus.LastSphereThrown -= StartNextTurn;
    }

    private void StartNextTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;

        if (IsPlayerTurn) BattleEventBus.NotifyPlayerTurnStarted();
        else BattleEventBus.NotifyOpponentTurnStarted();
    }
}