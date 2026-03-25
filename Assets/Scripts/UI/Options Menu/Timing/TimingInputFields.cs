using TMPro;
using UnityEngine;

public class TimingInputFields : MonoBehaviour
{
    [SerializeField] private TMP_InputField _restingStateDurationField;
    [SerializeField] private TMP_InputField _offBlockDurationField;

    [Header("Transitions")]
    [SerializeField] private TMP_InputField _captureSequenceDurationField;
    [SerializeField] private TMP_InputField _wakeupSequenceDurationField;


    private void Start()
    {
        Settings.RestingStateDuration.ConnectInputField(_restingStateDurationField);
        Settings.OffBlockDuration.ConnectInputField(_offBlockDurationField);

        Settings.CaptureSequenceDuration.ConnectInputField(_captureSequenceDurationField);
        Settings.WakeupSequenceDuration.ConnectInputField(_wakeupSequenceDurationField);
    }
}