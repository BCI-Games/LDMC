using UnityEngine;
using UnityEngine.UI;

public class TransitionOptionsHider: MonoBehaviour
{
    [SerializeField] private GameObject _captureOptionObject;
    [SerializeField] private GameObject _wakeupOptionObject;

    private Behaviour _layoutBehavior
        => _verticalLayoutGroup ??= GetComponent<VerticalLayoutGroup>();
    private VerticalLayoutGroup _verticalLayoutGroup;


    private void Start() => Settings.Modified += ShowRelevantTransitionOptions;
    private void OnDestroy() => Settings.Modified -= ShowRelevantTransitionOptions;
    
    private void OnEnable() => ShowRelevantTransitionOptions();

    private void ShowRelevantTransitionOptions()
    {
        bool showAny = Settings.CaptureSequenceEnabled || Settings.WakeupSequenceEnabled;
        foreach (Transform child in transform)
            child.gameObject.SetActive(showAny);
        
        _captureOptionObject.SetActive(Settings.CaptureSequenceEnabled);
        _wakeupOptionObject.SetActive(Settings.WakeupSequenceEnabled);

        Canvas.ForceUpdateCanvases();
        _layoutBehavior.enabled = false;
        _layoutBehavior.enabled = true;
    }
}