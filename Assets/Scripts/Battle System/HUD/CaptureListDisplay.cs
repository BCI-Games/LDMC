using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using static Easings;

public class CaptureListDisplay : MonoBehaviour
{
    [SerializeField] private MonsterSpawnController _monsterSpawner;
    [SerializeField] private Sprite _undiscoveredSprite;
    public float discovered_tween_scale = 2.0f;
    public float discovered_tween_grow_duration = 0.1f;
    public float discovered_tween_shrink_duration = 0.25f;

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
            if (display.RepresentsUndiscoveredMonster(capturedMonsterData))
            {
                display.ShowMonsterDiscovered();
                StartCoroutine(RunDisplayScaleTween(display));
            }
        }
    }

    private IEnumerator RunDisplayScaleTween(CaptureDisplay target)
    {
        this.StartTween(
            1, discovered_tween_scale, target.SetScale,
            discovered_tween_grow_duration,
            TransitionType.Back, EaseType.EaseOut
        );
        yield return new WaitForSeconds(discovered_tween_grow_duration);
        this.StartTween(
            discovered_tween_scale, 1, target.SetScale,
            discovered_tween_shrink_duration,
            TransitionType.Back, EaseType.EaseOut
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
        private MonsterData _monster;
        private Image _monsterImage;
        private Image _undiscoveredOverlay;
        private bool _monsterDiscovered;

        public CaptureDisplay(MonsterData monster)
        =>  _monster = monster;

        public bool RepresentsUndiscoveredMonster(MonsterData monster)
        => !_monsterDiscovered && monster == _monster;
        
        public void ShowMonsterDiscovered()
        {
            _monsterDiscovered = true;
            _monsterImage.color = Color.white;
            Destroy(_undiscoveredOverlay.gameObject);
            _undiscoveredOverlay = null;
        }

        public void SetScale(float scale)
        => _monsterImage.transform.localScale = Vector3.one * scale;


        public void Instantiate(Transform parent, Sprite undiscoveredIcon)
        {
            _monsterImage = CreateImageObject(
                _monster.IconSprite, parent, "Monster Image"
            );
            _monsterImage.color = Color.black;
            _undiscoveredOverlay = CreateImageObject(
                undiscoveredIcon, _monsterImage.transform, "Undiscovered Overlay"
            );
            _undiscoveredOverlay.rectTransform.sizeDelta = undiscoveredIcon.rect.size;
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
