using TMPro;
using UnityEngine;

public class BCIProcessingTimingFields : MonoBehaviour
{
    [SerializeField] private TMP_Text _epochCountLabel;
    [SerializeField] private TMP_InputField _epochLengthField;
    [SerializeField] private TMP_InputField _inputPollingPeriodField;

    private void Start()
    {
        _epochLengthField.text = Settings.EpochLength.ToString();
        _inputPollingPeriodField.text = Settings.InputPollingPeriod.ToString();

        _epochLengthField.onValueChanged.AddListener(SetEpochLength);
        _inputPollingPeriodField.onValueChanged.AddListener(SetInputPollingPeriod);

        Settings.AddAndInvokeModificationCallback(UpdateEpochCountDisplay);
    }

    private void OnDestroy() => Settings.Modified -= UpdateEpochCountDisplay;


    public void UpdateEpochCountDisplay()
    => _epochCountLabel.text = Settings.MinimumSharedEpochCount.ToString();


    private void SetEpochLength(string text)
    {
        if (float.TryParse(text, out float value))
            Settings.EpochLength = value;
    }

    private void SetInputPollingPeriod(string text)
    {
        if (float.TryParse(text, out float value))
            Settings.InputPollingPeriod = value;
    }
}