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
        Settings.OnBlockCycleCount.ConnectInputField(_onBlockCycleCountField);
        Settings.OnBlockEndedWithIdle.ConnectToggle(_onBlockEndedWithIdleToggle);

        Settings.CharacterActiveDuration.ConnectInputField(_activeDurationField);
        Settings.CharacterIdleDuration.ConnectInputField(_idleDurationField);

        Settings.AddAndInvokeModificationCallback(UpdateOnBlockDurationDisplay);
    }

    private void OnDestroy() => Settings.Modified -= UpdateOnBlockDurationDisplay;

    private void UpdateOnBlockDurationDisplay()
    => _onBlockDurationLabel.text = Settings.OnBlockDuration.ToString();
}
