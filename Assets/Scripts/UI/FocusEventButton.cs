using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FocusEventButton: Button
{
    public event Action HoverEntered;
    public event Action HoverExited;
    public event Action Selected;
    public event Action Deselected;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        HoverEntered?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        HoverExited?.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Selected?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Deselected?.Invoke();
    }
}