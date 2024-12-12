using UnityEngine;
using TMPro;

public class TimingInputFields: MonoBehaviour
{
    [SerializeField] private TMP_InputField _offBlockDurationField;

    [Header("On Block")]
    [SerializeField] private TMP_Text _onBlockDurationLabel;
    [SerializeField] private TMP_InputField _onBlockCycleCountField;

    [Header("Cycle Timings")]
    [SerializeField] private TMP_InputField _activeDurationField;
    [SerializeField] private TMP_InputField _idleDurationField;

    [Header("Transitions")]
    [SerializeField] private TMP_InputField _captureSequenceDurationField;
    [SerializeField] private TMP_InputField _wakeupSequenceDurationField;


    private void Start()
    {
        _offBlockDurationField.text = Settings.OffBlockDuration.ToString();
        _onBlockCycleCountField.text = Settings.OnBlockCycleCount.ToString();

        _activeDurationField.text = Settings.CharacterActiveDuration.ToString();
        _idleDurationField.text = Settings.CharacterIdleDuration.ToString();

        _captureSequenceDurationField.text = Settings.CaptureSequenceDuration.ToString();
        _wakeupSequenceDurationField.text = Settings.WakeupSequenceDuration.ToString();


        _offBlockDurationField.onValueChanged.AddListener(SetOffBlockDuration);
        _onBlockCycleCountField.onValueChanged.AddListener(SetOnBlockCycleCount);

        _activeDurationField.onValueChanged.AddListener(SetActiveDuration);
        _idleDurationField.onValueChanged.AddListener(SetIdleDuration);

        _captureSequenceDurationField.onValueChanged.AddListener(SetCaptureSequenceDuration);
        _wakeupSequenceDurationField.onValueChanged.AddListener(SetWakeupSequenceDuration);

        UpdateOnBlockDurationDisplay();
    }

    private void SetOffBlockDuration(string text)
        => Settings.OffBlockDuration = float.Parse(text);

    private void UpdateOnBlockDurationDisplay()
        => _onBlockDurationLabel.text = Settings.OnBlockDuration.ToString();
    private void SetOnBlockCycleCount(string text)
    {
        Settings.OnBlockCycleCount = int.Parse(text);
        UpdateOnBlockDurationDisplay();
    }
    private void SetActiveDuration(string text)
    {
        Settings.CharacterActiveDuration = float.Parse(text);
        UpdateOnBlockDurationDisplay();
    }
    private void SetIdleDuration(string text)
    {
        Settings.CharacterIdleDuration = float.Parse(text);
        UpdateOnBlockDurationDisplay();
    }

    private void SetCaptureSequenceDuration(string text)
        => Settings.CaptureSequenceDuration = float.Parse(text);
    private void SetWakeupSequenceDuration(string text)
        => Settings.WakeupSequenceDuration = float.Parse(text);
}