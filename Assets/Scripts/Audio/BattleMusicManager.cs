using UnityEngine;

public class BattleMusicManager : MonoBehaviour
{
    private void Start()
    {
        if (Settings.MusicPlayedInOffBlock)
        {
            Destroy(this);
            return;
        }

        BattleEventBus.RestPeriodStarted += VolumeManager.MuteMusic;
        BattleEventBus.RestPeriodEnded += VolumeManager.ResetMusicVolume;
    }

    private void OnDestroy()
    {
        BattleEventBus.RestPeriodStarted += VolumeManager.MuteMusic;
        BattleEventBus.RestPeriodEnded += VolumeManager.ResetMusicVolume;
    }
}