using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OptionsMenuTabButton: MonoBehaviour
{
    [SerializeField] private bool _selectOnStart;
    [SerializeField] private GameObject[] _linkedObjects;
    [SerializeField] private Transform _tabContentParent;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowLinkedObjects);
        if (_selectOnStart)
            ShowLinkedObjects();
    }

    public void ShowLinkedObjects()
    {
        foreach (Transform child in _tabContentParent)
            child.gameObject.SetActive(false);

        foreach (GameObject linkedObject in _linkedObjects)
            linkedObject.SetActive(true);
    }
}