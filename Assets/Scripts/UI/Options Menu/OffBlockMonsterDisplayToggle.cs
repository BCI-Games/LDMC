using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class OffBlockMonsterDisplayToggle: MonoBehaviour
{
    private Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Settings.OffBockMonsterDisplayEnabled;
        _toggle.onValueChanged.AddListener(SetOffBlockMonsterDisplayEnabled);
    }

    private void SetOffBlockMonsterDisplayEnabled(bool value)
    {
        Settings.OffBockMonsterDisplayEnabled = value;
    }
}