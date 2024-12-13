using UnityEngine;
using UnityEngine.UI;

public class FeedbackSequenceToggles: MonoBehaviour
{
    [SerializeField] private Toggle _captureSequenceToggle;
    [SerializeField] private Toggle _wakeupSequenceToggle;


    private void Start()
    {
        _captureSequenceToggle.isOn = Settings.CaptureSequenceEnabled;
        _wakeupSequenceToggle.isOn = Settings.WakeupSequenceEnabled;

        _captureSequenceToggle.onValueChanged.AddListener(SetCaptureSequenceEnabled);
        _wakeupSequenceToggle.onValueChanged.AddListener(SetWakeupSequenceEnabled);
    }

    private void SetCaptureSequenceEnabled(bool value)
        => Settings.CaptureSequenceEnabled = value;
    private void SetWakeupSequenceEnabled(bool value)
        => Settings.WakeupSequenceEnabled = value;
}