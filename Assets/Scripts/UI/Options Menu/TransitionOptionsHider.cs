using UnityEngine;
using UnityEngine.UI;

public class TransitionOptionsHider: DynamicLayoutBlock
{
    [SerializeField] private GameObject _captureOptionObject;
    [SerializeField] private GameObject _wakeupOptionObject;


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

        RefreshLayout();
    }
}