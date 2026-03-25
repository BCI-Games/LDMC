using TMPro;
using UnityEngine.UI;

public static class ValueProxyUIExtensions
{
    public static void ConnectInputField<T>
    (
        this ValueProxy<T> caller,
        TMP_InputField inputField
    )
    {
        inputField.text = caller.GetValue().ToString();
        inputField.onValueChanged.AddListener(text => caller.TrySetValue(text));
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

    public static void ConnectDropdown
    (
        this IntProxy caller,
        TMP_Dropdown dropdown
    )
    {
        dropdown.value = caller;
        dropdown.onValueChanged.AddListener(caller.SetValue);
    }

    public static void ConnectToggle
    (
        this BooleanProxy caller,
        Toggle inputField
    )
    {
        inputField.isOn = caller;
        inputField.onValueChanged.AddListener(caller.SetValue);
    }
}