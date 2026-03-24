using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnBlockTimingFields : MonoBehaviour
{
    [SerializeField] private TMP_Text _onBlockDurationLabel;
    [SerializeField] private TMP_InputField _onBlockCycleCountField;
    [SerializeField] private Toggle _onBlockEndedWithIdleToggle;

    [Header("Cycle Timings")]
    [SerializeField] private TMP_InputField _activeDurationField;
    [SerializeField] private TMP_InputField _idleDurationField;


    private void Start()
    {

        _onBlockCycleCountField.text = Settings.OnBlockCycleCount.ToString();
        _onBlockEndedWithIdleToggle.isOn = Settings.OnBlockEndedWithIdle;

        _activeDurationField.text = Settings.CharacterActiveDuration.ToString();
        _idleDurationField.text = Settings.CharacterIdleDuration.ToString();

        _onBlockCycleCountField.onValueChanged.AddListener(SetOnBlockCycleCount);
        _onBlockEndedWithIdleToggle.onValueChanged.AddListener(SetOnBlockEndedWithIdle);

        _activeDurationField.onValueChanged.AddListener(SetActiveDuration);
        _idleDurationField.onValueChanged.AddListener(SetIdleDuration);

        UpdateOnBlockDurationDisplay();
    }

    private void UpdateOnBlockDurationDisplay()
    => _onBlockDurationLabel.text = Settings.OnBlockDuration.ToString();


    private void SetOnBlockCycleCount(string text)
    {
        if (int.TryParse(text, out int value))
        {
            Settings.OnBlockCycleCount = value;
            UpdateOnBlockDurationDisplay();
        }
    }
    private void SetOnBlockEndedWithIdle(bool value)
    {
        Settings.OnBlockEndedWithIdle = value;
        UpdateOnBlockDurationDisplay();
    }

    private void SetActiveDuration(string text)
    {
        if (float.TryParse(text, out float value))
        {
            Settings.CharacterActiveDuration = value;
            UpdateOnBlockDurationDisplay();
        }
    }
    private void SetIdleDuration(string text)
    {
        if (float.TryParse(text, out float value))
        {
            Settings.CharacterIdleDuration = value;
            UpdateOnBlockDurationDisplay();
        }
    }
}
