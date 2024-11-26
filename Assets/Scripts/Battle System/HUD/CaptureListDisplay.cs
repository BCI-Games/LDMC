using UnityEngine;
using UnityEngine.UI;

public class CaptureListDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _monsterIconPrefab;


    private void Start() => BattleEventBus.MonsterCaptured += AddMonsterIcon;
    private void OnDestroy() => BattleEventBus.MonsterCaptured -= AddMonsterIcon;


    public void AddMonsterIcon(MonsterData capturedMonsterData)
    {
        GameObject newIconObject = Instantiate(_monsterIconPrefab, transform);
        newIconObject.GetComponent<Image>().sprite = capturedMonsterData.IconSprite;
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
}
