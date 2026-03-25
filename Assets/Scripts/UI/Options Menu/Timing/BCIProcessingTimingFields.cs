using TMPro;
using UnityEngine;

public class BCIProcessingTimingFields : MonoBehaviour
{
    [SerializeField] private TMP_Text _epochCountLabel;
    [SerializeField] private TMP_InputField _epochLengthField;
    [SerializeField] private TMP_InputField _inputPollingPeriodField;

    private void Start()
    {
        Settings.EpochLength.ConnectInputField(_epochLengthField);
        Settings.InputPollingPeriod.ConnectInputField(_inputPollingPeriodField);

        Settings.AddAndInvokeModificationCallback(UpdateEpochCountDisplay);
    }

    private void OnDestroy() => Settings.Modified -= UpdateEpochCountDisplay;


    public void UpdateEpochCountDisplay()
    => _epochCountLabel.text = Settings.MinimumSharedEpochCount.ToString();
}