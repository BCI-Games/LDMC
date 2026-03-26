using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsFieldInputLink : MonoBehaviour
{
    [SerializeField] private string TargetFieldName;


    public void Start()
    {
        TargetFieldName = TargetFieldName.Replace(" ", "");
        if (!Settings.HasValue(TargetFieldName))
        {
            Debug.LogWarning($"Target setting \"{TargetFieldName}\" not found.");
            return;
        }

        switch (GetComponentInChildren<Selectable>())
        {
            case null:
                Debug.LogError("Missing input element");
                break;
            case TMP_InputField inputField:
                Settings.TryGetValue(TargetFieldName, out ValueProxy inputTarget);
                inputTarget.ConnectInputField(inputField);
                break;
            case TMP_Dropdown dropdown:
                Settings.TryGetValue(TargetFieldName, out IntProxy dropdownTarget);
                dropdownTarget.ConnectDropdown(dropdown);
                break;
            case Slider slider:
                Settings.TryGetValue(TargetFieldName, out FloatProxy sliderTarget);
                sliderTarget.ConnectSlider(slider);
                break;
            case Toggle toggle:
                Settings.TryGetValue(TargetFieldName, out BooleanProxy toggleTarget);
                toggleTarget.ConnectToggle(toggle);
                break;
            default:
                Debug.LogError("Unsupported input element");
                break;
        }
    }
}