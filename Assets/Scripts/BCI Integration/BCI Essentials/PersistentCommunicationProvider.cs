using BCIEssentials.Behaviours;
using UnityEngine.SceneManagement;

public class PersistentCommunicationProvider : CommunicationProvider
{
    private PersistentCommunicationProvider _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            InitializeProvisionTriggers();
        }
        else Destroy(gameObject);
    }

    private void InitializeProvisionTriggers()
    {
        if (ProvisionTriggers.HasFlag(ProvisionOccasion.SceneLoad))
        {
            SceneManager.activeSceneChanged += (_, _) => ProvideCommunication();
        }
        else if (ProvisionTriggers.HasFlag(ProvisionOccasion.Awake))
        {
            ProvideCommunication();
        }
    }
}