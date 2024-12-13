using UnityEngine;
using UnityEngine.UI;

public class DynamicLayoutBlock: MonoBehaviour
{
    private Behaviour _layoutBehavior
        => _verticalLayoutGroup ??= GetComponent<LayoutGroup>();
    private LayoutGroup _verticalLayoutGroup;

    public void RefreshLayout()
    {
        Canvas.ForceUpdateCanvases();
        _layoutBehavior.enabled = false;
        _layoutBehavior.enabled = true;
    }
}