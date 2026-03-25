using UnityEngine;
using UnityEngine.UI;

public class FeedbackSequenceToggles: MonoBehaviour
{
    [SerializeField] private Toggle _captureSequenceToggle;
    [SerializeField] private Toggle _wakeupSequenceToggle;


    private void Start()
    {
        Settings.CaptureSequenceEnabled.ConnectToggle(_captureSequenceToggle);
        Settings.WakeupSequenceEnabled.ConnectToggle(_wakeupSequenceToggle);
    }
}