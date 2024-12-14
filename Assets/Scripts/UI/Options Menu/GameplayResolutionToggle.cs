using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class GameplayResolutionToggle: MonoBehaviour
{
    private Toggle _toggle;


    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Settings.PixelPerfectCameraEnabled;
        _toggle.onValueChanged.AddListener(SetPixelPerfectCameraEnabled);
    }

    private void SetPixelPerfectCameraEnabled(bool value)
    {
        Settings.PixelPerfectCameraEnabled = value;
    }
}