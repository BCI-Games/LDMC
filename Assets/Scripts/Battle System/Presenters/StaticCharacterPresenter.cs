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
    private bool _isResting;


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


    protected override void ShowWindupStarted()
        => SetSpriteIfNotResting(_activeSprite);
    protected override void ShowWindupCancelled()
        => SetSpriteIfNotResting(_idleSprite);
    protected override void ShowThrow()
        => SetSpriteIfNotResting(_idleSprite);

    private void SetSpriteIfNotResting(Sprite newSprite)
    {
        if (_isResting) return;
        _renderer.sprite = newSprite;
    }

    protected override void ShowRestEnded()
    {
        _isResting = false;
        _renderer.sprite = _idleSprite;
        _sleepyZObject.SetActive(false);
    }
    
    protected override void ShowRestStarted()
    {
        _isResting = true;
        _renderer.sprite = _restSprite;
    }

    protected override IEnumerator RunRestCycle()
    {
        _sleepyZObject.SetActive(true);
        yield return new WaitForSeconds(Settings.CharacterActiveDuration);
        _sleepyZObject.SetActive(false);
        yield return new WaitForSeconds(Settings.CharacterIdleDuration);
    }
}