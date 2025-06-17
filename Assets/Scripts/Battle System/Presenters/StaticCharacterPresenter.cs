using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class StaticCharacterPresenter : CharacterPresenter
{
    [SerializeField] private GameObject _sleepyZObject;

    [Header("Sprites")]
    [SerializeField] private SpriteWithOffset _idleSprite;
    [SerializeField] private SpriteWithOffset _activeSprite;
    [SerializeField] private SpriteWithOffset _restSprite;

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

    private void SetSpriteIfNotResting(SpriteWithOffset newSprite)
    {
        if (_isResting) return;
        SetSprite(newSprite);
    }

    private void SetSprite(SpriteWithOffset newSprite)
    {
        _renderer.sprite = newSprite.Sprite;
        transform.localPosition = newSprite.Offset;
    }

    protected override void ShowRestEnded()
    {
        _isResting = false;
        SetSprite(_idleSprite);
        _sleepyZObject.SetActive(false);
    }

    protected override void ShowRestStarted()
    {
        _isResting = true;
        SetSprite(_restSprite);
    }

    protected override IEnumerator RunRestCycle()
    {
        _sleepyZObject.SetActive(true);
        yield return new WaitForSeconds(Settings.CharacterActiveDuration);
        _sleepyZObject.SetActive(false);
        yield return new WaitForSeconds(Settings.CharacterIdleDuration);
    }

    [System.Serializable]
    public class SpriteWithOffset
    {
        public Sprite Sprite;
        public Vector2 Offset = Vector2.zero;
    }
}