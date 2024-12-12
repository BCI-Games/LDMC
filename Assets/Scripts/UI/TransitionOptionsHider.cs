using UnityEngine;

public class TransitionOptionsHider: MonoBehaviour
{
    [SerializeField] private GameObject _captureOptionObject;
    [SerializeField] private GameObject _wakeupOptionObject;

    private bool _wasActivated;


    private void Start() => Settings.Modified += ShowRelevantTransitionOptions;
    private void OnDestroy() => Settings.Modified -= ShowRelevantTransitionOptions;
    
    private void OnEnable() {
        _wasActivated = true;
        ShowRelevantTransitionOptions();
    }

    private void OnDisable() {
        _wasActivated = false;
    }

    private void ShowRelevantTransitionOptions()
    {
        gameObject.SetActive(_wasActivated && (Settings.CaptureSequenceEnabled || Settings.WakeupSequenceEnabled));
        _captureOptionObject.SetActive(Settings.CaptureSequenceEnabled);
        _wakeupOptionObject.SetActive(Settings.WakeupSequenceEnabled);
    }
}