using TMPro;
using UnityEngine.UI;

public static class ValueProxyUIExtensions
{
    public static void ConnectInputField
    (
        this ValueProxy caller,
        TMP_InputField inputField
    )
    {
        inputField.text = caller.ToString();
        inputField.onValueChanged.AddListener(text => caller.TrySetValue(text));
    }

    public static void ConnectDropdown
    (
        this IntProxy caller,
        TMP_Dropdown dropdown
    )
    {
        dropdown.value = caller;
        dropdown.onValueChanged.AddListener(caller.SetValue);
    }

    public static void ConnectSlider
    (
        this FloatProxy caller,
        Slider slider
    )
    {
        slider.value = caller;
        slider.onValueChanged.AddListener(caller.SetValue);
    }

    public static void ConnectToggle
    (
        this BooleanProxy caller,
        Toggle toggle
    )
    {
        toggle.isOn = caller;
        toggle.onValueChanged.AddListener(caller.SetValue);
    }
}