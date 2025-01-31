using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FocusButtonTooltipLabel: MonoBehaviour
{
    [SerializeField] private LabelledButton[] _labelledButtons;

    private TextMeshProUGUI _label;
    private string _selectedButtonTooltip;


    private void Start()
    {
        _label = GetComponent<TextMeshProUGUI>();
        _label.text = "";

        foreach (LabelledButton labelledButton in _labelledButtons)
        {
            FocusEventButton button = labelledButton.button;
            string tooltip = labelledButton.tooltip;

            button.HoverEntered += BindButtonOnHoverEntered(tooltip);
            button.HoverExited += OnButtonHoverExited;
            button.Selected += BindOnButtonSelected(tooltip);
            button.Deselected += OnButtonDeselected;
        }
    }

    private Action BindButtonOnHoverEntered(string tooltip)
        => () => _label.text = tooltip;
    private void OnButtonHoverExited()
        => _label.text = _selectedButtonTooltip ?? "";

    private Action BindOnButtonSelected(string tooltip)
        => () => SetLabelAndSelectedText(tooltip);
    private void OnButtonDeselected()
        => SetLabelAndSelectedText("");

    private void SetLabelAndSelectedText(string tooltip)
    {
        _label.text = _selectedButtonTooltip = tooltip;
    }


    [Serializable]
    public struct LabelledButton
    {
        public FocusEventButton button;
        public string tooltip;
    }
}