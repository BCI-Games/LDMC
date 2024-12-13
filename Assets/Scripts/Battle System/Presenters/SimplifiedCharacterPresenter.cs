using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimplifiedCharacterPresenter: MonoBehaviour
{
    [SerializeField] private GameObject _sleepyZObject;

    [Header("Sprite")]
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _restSprite;

    private SpriteRenderer _renderer;
    protected Sprite Sprite { set => _renderer.sprite = value; }

    
    protected virtual void Start()
    {
        if (!Settings.AnimationSimplified)
        {
            Destroy(gameObject);
            return;
        }

        _renderer = GetComponent<SpriteRenderer>();

        BattleEventBus.WindupStarted += ShowActiveState;
        BattleEventBus.WindupCancelled += ShowIdleState;
        BattleEventBus.SphereThrown += ShowIdleState;
        BattleEventBus.OffBlockStarted += ShowRestState;
    }
    protected virtual void OnDestroy()
    {
        BattleEventBus.WindupStarted -= ShowActiveState;
        BattleEventBus.WindupCancelled -= ShowIdleState;
        BattleEventBus.SphereThrown -= ShowIdleState;
        BattleEventBus.OffBlockStarted -= ShowRestState;
    }


    protected void ShowIdleState()
    {
        if (!BlockManager.IsOnBlock) return;
        Sprite = _idleSprite;
        _sleepyZObject.SetActive(false);
    }
    protected void ShowActiveState()
    {
        if (!BlockManager.IsOnBlock) return;
        Sprite = _activeSprite;
        _sleepyZObject.SetActive(false);
    }
    protected void ShowRestState()
    {
        Sprite = _restSprite;
        _sleepyZObject.SetActive(true);
    }
}