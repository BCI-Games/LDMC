using UnityEngine;
using System.Collections;

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
        BattleEventBus.OffBlockStarted += StartRestCycle;
    }
    protected virtual void OnDestroy()
    {
        BattleEventBus.WindupStarted -= ShowActiveState;
        BattleEventBus.WindupCancelled -= ShowIdleState;
        BattleEventBus.SphereThrown -= ShowIdleState;
        BattleEventBus.OffBlockStarted -= StartRestCycle;
    }


    protected void ShowIdleState()
    {
        if (!BlockManager.IsOnBlock) return;
        Sprite = _idleSprite;
    }
    protected void ShowActiveState()
    {
        if (!BlockManager.IsOnBlock) return;
        Sprite = _activeSprite;
    }


    private void StartRestCycle()
    {
        Sprite = _restSprite;
        StartCoroutine(RunRestCycle());
    }

    private IEnumerator RunRestCycle()
    {
        while(!BlockManager.IsOnBlock)
        {
            _sleepyZObject.SetActive(true);
            yield return new WaitForSeconds(Settings.CharacterIdleDuration);
            _sleepyZObject.SetActive(false);
            yield return new WaitForSeconds(Settings.CharacterIdleDuration);
        }
        _sleepyZObject.SetActive(false);
    }
}