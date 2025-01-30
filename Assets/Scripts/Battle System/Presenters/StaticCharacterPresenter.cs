using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class StaticCharacterPresenter: CharacterPresenter
{
    [SerializeField] private GameObject _sleepyZObject;

    [Header("Sprites")]
    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _restSprite;
    
    private SpriteRenderer _renderer;
    protected Sprite Sprite { set => _renderer.sprite = value; }


    protected override void Start()
    {
        if (!Settings.AnimationSimplified)
        {
            Destroy(gameObject);
            return;
        }
        _renderer = GetComponent<SpriteRenderer>();

        SubscribeToBattleEvents();
    }


    protected override void ShowWindupStarted() => Sprite = _activeSprite;
    protected override void ShowWindupCancelled() => Sprite = _idleSprite;
    protected override void ShowThrow() => Sprite = _idleSprite;

    protected override void ShowRestEnded()
    {
        Sprite = _idleSprite;
        _sleepyZObject.SetActive(false);
    }
    
    protected override void ShowRestStarted() => Sprite = _restSprite;

    protected override IEnumerator RunRestCycle()
    {
        _sleepyZObject.SetActive(true);
        yield return new WaitForSeconds(Settings.CharacterActiveDuration);
        _sleepyZObject.SetActive(false);
        yield return new WaitForSeconds(Settings.CharacterIdleDuration);
    }
}