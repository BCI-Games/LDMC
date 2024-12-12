using UnityEngine;

public class SettingsLoadTrigger: MonoBehaviour
{
    private void Awake()
    {
        Settings.LoadAndApplySettings();
        Destroy(gameObject);
    }
}