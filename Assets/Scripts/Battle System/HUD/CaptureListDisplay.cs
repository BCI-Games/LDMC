using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using static Easings;

public class CaptureListDisplay : MonoBehaviour
{
    [SerializeField] private MonsterSpawnController _monsterSpawner;
    [SerializeField] private Sprite _undiscoveredSprite;

    public float IconTweenDiscoveryScale = 1.5f;
    public float IconTweenRecaptureScale = 1.25f;
    public float IconTweenGrowPeriod = 0.1f;
    public float IconTweenHoldPeriod = 0.1f;
    public float IconTweenShrinkPeriod = 0.5f;

    private CaptureDisplay[] _displays;


    private void Start()
    {
        if (!_monsterSpawner) _monsterSpawner = FindAnyObjectByType<MonsterSpawnController>();
        CreateIcons();
        BattleEventBus.MonsterCaptured += ShowMonsterIcon;
    }
    private void OnDestroy() => BattleEventBus.MonsterCaptured -= ShowMonsterIcon;


    public void ShowMonsterIcon(MonsterData capturedMonsterData)
    {
        foreach(CaptureDisplay display in _displays)
        {
            if (display.RepresentsMonster(capturedMonsterData))
            {
                display.ShowMonsterDiscovered();
                StartCoroutine(RunDisplayScaleTween(display));
                display.MonsterDiscovered = true;
            }
        }
    }

    private IEnumerator RunDisplayScaleTween(CaptureDisplay target)
    {
        target.StartScaleTween(
            target.MonsterDiscovered
            ? IconTweenRecaptureScale
            : IconTweenDiscoveryScale,
            IconTweenGrowPeriod,
            TransitionType.Back, EaseType.EaseOut
        );
        yield return new WaitForSeconds(IconTweenGrowPeriod + IconTweenHoldPeriod);
        target.StartScaleTween(
            1, IconTweenShrinkPeriod,
            TransitionType.Elastic, EaseType.EaseOut
        );
    }

    private void CreateIcons()
    {
        MonsterData[] monsterList = _monsterSpawner.Monsters;
        _displays = new CaptureDisplay[monsterList.Length];
        for (int i = 0; i < monsterList.Length; i++)
        {
            _displays[i] = new(monsterList[i]);
            _displays[i].Instantiate(transform, _undiscoveredSprite);
        }
    }


    private class CaptureDisplay
    {
        public bool MonsterDiscovered;
        private MonsterData _monster;
        private Image _monsterImage;
        private Image _undiscoveredOverlay;

        public CaptureDisplay(MonsterData monster)
        =>  _monster = monster;

        public bool RepresentsMonster(MonsterData monster)
        => monster == _monster;
        
        public void ShowMonsterDiscovered()
        {
            _monsterImage.sprite = _monster.IconSprite;
            _monsterImage.color = Color.white;
            if (!_undiscoveredOverlay) return;
            Destroy(_undiscoveredOverlay.gameObject);
            _undiscoveredOverlay = null;
        }

        public void StartScaleTween(
            float scale, float period,
            TransitionType transition, EaseType easing
        )
        => _monsterImage.StartScaleTween(scale, period, transition, easing);


        public void Instantiate(Transform parent, Sprite undiscoveredIcon)
        {
            _monsterImage = CreateImageObject(
                _monster.UndiscoveredIconSprite, parent, "Monster Image"
            );
            _monsterImage.color = Color.black;
            _undiscoveredOverlay = CreateImageObject(
                undiscoveredIcon, _monsterImage.transform, "Undiscovered Overlay"
            );
            RectTransform overlayRect = _undiscoveredOverlay.rectTransform;
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.anchoredPosition = Vector2.zero;
            overlayRect.sizeDelta = Vector2.zero;
        }

        private static Image CreateImageObject(
            Sprite sprite, Transform parent, string name = ""
        )
        {
            GameObject imageObject = new(name);
            Transform imageTransform = imageObject.AddComponent<RectTransform>();
            imageTransform.SetParent(parent);
            imageTransform.localScale = Vector3.one;
            Image image = imageObject.AddComponent<Image>();
            image.sprite = sprite;
            return image;
        }
    }
}
