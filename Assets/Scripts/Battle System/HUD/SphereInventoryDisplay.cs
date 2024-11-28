using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereInventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _sphereIconPrefab;

    private List<GameObject> _sphereIcons;
    private int _currentSphereIndex = 0;


    private void Start() => BattleEventBus.SphereThrown += DecreaseIconCount;
    private void OnDestroy() => BattleEventBus.SphereThrown -= DecreaseIconCount;


    public void BuildSphereIcons(int sphereCount)
    {
        _sphereIcons = new();
        for (int index = 0; index < sphereCount; index ++)
        {
            GameObject newIcon = Instantiate(_sphereIconPrefab, transform);
            newIcon.name = "Sphere Icon " + index;
            _sphereIcons.Add(newIcon);
        }
    }

    public void ClearIcons()
    {
        foreach(GameObject childIcon in _sphereIcons)
            Destroy(childIcon);
        _sphereIcons.Clear();
    }

    public void ResetSphereCount(int sphereCount)
    {
        ClearIcons();
        BuildSphereIcons(sphereCount);
        _currentSphereIndex = 0;
    }

    public void ResetIcons()
    {
        foreach (GameObject item in _sphereIcons)
        {
            item.GetComponent<Image>().color = Color.white;
        }
        _currentSphereIndex = 0;
    }

    private void DecreaseIconCount()
    {
        if (_currentSphereIndex < _sphereIcons.Count)
        {
            _sphereIcons[_currentSphereIndex].GetComponent<Image>().color = Color.black;
            _currentSphereIndex++;
        }
    }
}
