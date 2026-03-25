using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SettingsFieldInputLink : MonoBehaviour
{
    [SerializeField] private string TargetFieldName;


    public void Start()
    {
        TargetFieldName = TargetFieldName.Replace(" ", "");
        if(!Settings.HasField(TargetFieldName))
        {
            Debug.LogWarning($"Target setting \"{TargetFieldName}\" not found.");
            return;
        }

        switch (GetComponent<Selectable>())
        {
            case TMP_InputField inputField:
                Settings.GetField(TargetFieldName, out ValueProxy inputTarget);
                inputTarget.ConnectInputField(inputField);
                break;
            case TMP_Dropdown dropdown:
                Settings.GetField(TargetFieldName, out IntProxy dropdownTarget);
                dropdownTarget.ConnectDropdown(dropdown);
                break;
            case Slider slider:
                Settings.GetField(TargetFieldName, out FloatProxy sliderTarget);
                sliderTarget.ConnectSlider(slider);
                break;
            case Toggle toggle:
                Settings.GetField(TargetFieldName, out BooleanProxy toggleTarget);
                toggleTarget.ConnectToggle(toggle);
                break;
            default:
                Debug.LogError("Unsupported input element");
                break;
        }
    }
}