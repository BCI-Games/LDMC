using TMPro;
using UnityEngine;

public class TimingInputFields : MonoBehaviour
{
    [SerializeField] private TMP_InputField _offBlockDurationField;

    [Header("Transitions")]
    [SerializeField] private TMP_InputField _captureSequenceDurationField;
    [SerializeField] private TMP_InputField _wakeupSequenceDurationField;


    private void Start()
    {
        _offBlockDurationField.text = Settings.OffBlockDuration.ToString();

        _captureSequenceDurationField.text = Settings.CaptureSequenceDuration.ToString();
        _wakeupSequenceDurationField.text = Settings.WakeupSequenceDuration.ToString();


        _offBlockDurationField.onValueChanged.AddListener(SetOffBlockDuration);

        _captureSequenceDurationField.onValueChanged.AddListener(SetCaptureSequenceDuration);
        _wakeupSequenceDurationField.onValueChanged.AddListener(SetWakeupSequenceDuration);
    }

    private void SetOffBlockDuration(string text)
    {
        if (float.TryParse(text, out float value))
            Settings.OffBlockDuration = value;
    }

    private void SetCaptureSequenceDuration(string text)
    {
        if (float.TryParse(text, out float value))
            Settings.CaptureSequenceDuration = value;
    }
    private void SetWakeupSequenceDuration(string text)
    {
        if (float.TryParse(text, out float value))
            Settings.WakeupSequenceDuration = value;
    }
}