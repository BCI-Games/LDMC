using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SettingsPropertyLabel : MonoBehaviour
{
    [SerializeField, SettingsPropertyName]
    private string _targetPropertyName;
    private TMP_Text _label;


    private void Start()
    {
        _targetPropertyName = _targetPropertyName.Replace(" ", "");
        if (!Settings.HasProperty(_targetPropertyName))
        {
            Debug.LogWarning($"Target property \"{_targetPropertyName}\" not found.");
            return;
        }

        _label = GetComponent<TMP_Text>();
        Settings.AddAndInvokeModificationCallback(UpdateLabel);
    }

    private void OnDestroy() => Settings.Modified -= UpdateLabel;


    private void UpdateLabel()
    {
        Settings.TryGetPropertyValue(_targetPropertyName, out object value);
        _label.text = value.ToString();
    }
}

public class SettingsPropertyNameAttribute : PropertyAttribute { }