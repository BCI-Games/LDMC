using UnityEngine;
using BCIEssentials.LSLFramework;

public class PersistentMarkerStream: LSLMarkerStream
{
    public static bool CanPush => _instance;
    static PersistentMarkerStream _instance;

    void Awake()
    {
        if (_instance is null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
            Destroy(this);
    }

    public static void PushString(string markerString)
    {
        if (_instance)
            _instance.Write(markerString);
        else
            Debug.LogWarning("No marker stream to push to");
    }
}