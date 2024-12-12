using UnityEngine;
using UnityEngine.UI;

public class DisplayToggleButtons: MonoBehaviour
{
    [SerializeField] private Toggle _meshAnimationToggle;
    [SerializeField] private Toggle _captureSequenceToggle;
    [SerializeField] private Toggle _wakeupSequenceToggle;

    
    private void Start()
    {
        _meshAnimationToggle.isOn = Settings.MeshAnimationEnabled;
        _captureSequenceToggle.isOn = Settings.CaptureSequenceEnabled;
        _wakeupSequenceToggle.isOn = Settings.WakeupSequenceEnabled;

        _meshAnimationToggle.onValueChanged.AddListener(SetMeshAnimationEnabled);
        _captureSequenceToggle.onValueChanged.AddListener(SetCaptureSequenceEnabled);
        _wakeupSequenceToggle.onValueChanged.AddListener(SetWakeupSequenceEnabled);
    }

    private void SetMeshAnimationEnabled(bool value)
        => Settings.MeshAnimationEnabled = value;
    private void SetCaptureSequenceEnabled(bool value)
        => Settings.CaptureSequenceEnabled = value;
    private void SetWakeupSequenceEnabled(bool value)
        => Settings.WakeupSequenceEnabled = value;
}