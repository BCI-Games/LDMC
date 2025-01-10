using UnityEngine;

public class PauseManager: MonoBehaviour
{
    [SerializeField] private GameObject _pauseOverlay;

    public bool IsPaused {
        get => _isPaused;
        set {
            _isPaused = value;
            Time.timeScale = value? 0: 1;
            _pauseOverlay.SetActive(value);
            BattleEventBus.NotifyPauseToggled(value);
        }
    }
    private bool _isPaused;


    private void Start() => IsPaused = true;
    private void OnDestroy() => IsPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            IsPaused = !IsPaused;
    }
}