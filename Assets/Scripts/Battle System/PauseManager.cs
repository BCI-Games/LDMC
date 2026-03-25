using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseOverlay;
    private bool _isPaused;


    private void Start() => Pause();
    private void OnDestroy() => Unpause();


    public void Pause() => SetPaused(true);
    public void Unpause() => SetPaused(false);
    public void SetPaused(bool value)
    {
        _isPaused = value;
        Time.timeScale = value ? 0 : 1;
        _pauseOverlay.SetActive(value);
        BattleEventBus.NotifyPauseToggled(value);
    }
}