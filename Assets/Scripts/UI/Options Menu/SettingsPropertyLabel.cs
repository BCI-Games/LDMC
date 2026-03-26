using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SettingsPropertyLabel : MonoBehaviour
{
    [SerializeField] private string TargetPropertyName;
    private TMP_Text _label;


    private void Start()
    {
        TargetPropertyName = TargetPropertyName.Replace(" ", "");
        if (!Settings.HasProperty(TargetPropertyName))
        {
            Debug.LogWarning($"Target property \"{TargetPropertyName}\" not found.");
            return;
        }

        _label = GetComponent<TMP_Text>();
        Settings.AddAndInvokeModificationCallback(UpdateLabel);
    }

    private void OnDestroy() => Settings.Modified -= UpdateLabel;


    private void UpdateLabel()
    {
        Settings.TryGetPropertyValue(TargetPropertyName, out object value);
        _label.text = value.ToString();
    }
}