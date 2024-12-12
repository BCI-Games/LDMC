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
    {
        float value;
        if (float.TryParse(text, out value))
            Settings.OffBlockDuration = value;
    }

    private void UpdateOnBlockDurationDisplay()
        => _onBlockDurationLabel.text = Settings.OnBlockDuration.ToString();
    private void SetOnBlockCycleCount(string text)
    {
        
        int value;
        if (int.TryParse(text, out value))
        {
            Settings.OnBlockCycleCount = value;
            UpdateOnBlockDurationDisplay();
        }
    }
    private void SetActiveDuration(string text)
    {
        
        float value;
        if (float.TryParse(text, out value))
        {
            Settings.CharacterActiveDuration = value;
            UpdateOnBlockDurationDisplay();
        }
    }
    private void SetIdleDuration(string text)
    {
        
        float value;
        if (float.TryParse(text, out value))
        {
            Settings.CharacterIdleDuration = value;
            UpdateOnBlockDurationDisplay();
        }
    }

    private void SetCaptureSequenceDuration(string text)
    {
        float value;
        if (float.TryParse(text, out value))
            Settings.CaptureSequenceDuration = value;
    }
    private void SetWakeupSequenceDuration(string text)
    {
        float value;
        if (float.TryParse(text, out value))
            Settings.WakeupSequenceDuration = value;
    }
}