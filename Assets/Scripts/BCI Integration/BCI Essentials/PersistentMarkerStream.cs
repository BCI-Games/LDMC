using UnityEngine;
using BCIEssentials.LSLFramework;

public class PersistentMarkerStream: LSLMarkerStream
{
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
}