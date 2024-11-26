using UnityEngine;
using UnityEngine.UI;



public class CaptureListDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _monsterIconPrefab;


    private void Start() => BattleEventBus.MonsterCaptured += OnMonsterCaptured;
    private void OnDestroy() => BattleEventBus.MonsterCaptured -= OnMonsterCaptured;


    public void AddMonsterIcon(Sprite monsterIcon)
    {
        GameObject newIconObject = Instantiate(_monsterIconPrefab, transform);
        newIconObject.GetComponent<Image>().sprite = monsterIcon;
    }

    public void ClearIcons()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveLastIcon()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }

    private void OnMonsterCaptured(MonsterData monsterData)
    {
        AddMonsterIcon(monsterData.IconSprite);
    }
}
