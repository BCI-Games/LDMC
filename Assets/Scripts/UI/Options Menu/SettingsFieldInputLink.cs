using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsFieldInputLink : MonoBehaviour
{
    [SerializeField, SettingsFieldName]
    private string _targetFieldName;


    public void Start()
    {
        _targetFieldName = _targetFieldName.Replace(" ", "");
        if (!Settings.HasValue(_targetFieldName))
        {
            Debug.LogWarning($"Target setting \"{_targetFieldName}\" not found.");
            return;
        }

        switch (GetComponentInChildren<Selectable>())
        {
            case null:
                Debug.LogError("Missing input element");
                break;
            case TMP_InputField inputField:
                Settings.TryGetValue(_targetFieldName, out ValueProxy inputTarget);
                inputTarget.ConnectInputField(inputField);
                break;
            case TMP_Dropdown dropdown:
                Settings.TryGetValue(_targetFieldName, out IntProxy dropdownTarget);
                dropdownTarget.ConnectDropdown(dropdown);
                break;
            case Slider slider:
                Settings.TryGetValue(_targetFieldName, out FloatProxy sliderTarget);
                sliderTarget.ConnectSlider(slider);
                break;
            case Toggle toggle:
                Settings.TryGetValue(_targetFieldName, out BooleanProxy toggleTarget);
                toggleTarget.ConnectToggle(toggle);
                break;
            default:
                Debug.LogError("Unsupported input element");
                break;
        }
    }
}

public class SettingsFieldNameAttribute : PropertyAttribute { }