using UnityEngine;

public class CharacterPresenter: Tweener
{
    public static float ThrowChargeLevel {
        set {
            if (CurrentCharacterAnimator)
                CurrentCharacterAnimator.SetFloat("Charge Level", value);
        }
    }
    protected static Animator CurrentCharacterAnimator;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _opponentTransform;

    [Header("Swap Animation")]
    [SerializeField] private Vector2 _inactiveOffset = new(-10, -1);
    [SerializeField] private float _tweenPeriod = 0.5f;
    [SerializeField] private TransitionType _swapTransition = TransitionType.Back;
    [SerializeField] private EaseType _swapEasing = EaseType.EaseOut;


    private void Start()
    {
        _playerTransform.localPosition = _inactiveOffset;
        _opponentTransform.localPosition = _inactiveOffset;

        if (TurnManager.IsPlayerTurn) SwapPlayerIn(); else SwapOpponentIn();

        BattleEventBus.PlayerTurnStarted += SwapPlayerIn;
        BattleEventBus.OpponentTurnStarted += SwapOpponentIn;
        BattleEventBus.SphereThrown += PlayThrowAnimation;
    }

    private void OnDestroy()
    {
        BattleEventBus.PlayerTurnStarted -= SwapPlayerIn;
        BattleEventBus.OpponentTurnStarted -= SwapOpponentIn;
        BattleEventBus.SphereThrown -= PlayThrowAnimation;
    }


    private void SwapPlayerIn() => StartSwapTween(_playerTransform, _opponentTransform);
    private void SwapOpponentIn() => StartSwapTween(_opponentTransform, _playerTransform);

    private void StartSwapTween(Transform activeCharacter, Transform inactiveCharacter)
    {
        CurrentCharacterAnimator = activeCharacter.GetComponent<Animator>();
        StartPositionTween(activeCharacter, Vector2.zero, _tweenPeriod, _swapTransition, _swapEasing);
        StartPositionTween(inactiveCharacter, _inactiveOffset, _tweenPeriod, _swapTransition, _swapEasing);
    }


    private void PlayThrowAnimation()
    {
        CurrentCharacterAnimator.SetTrigger("Throw");
    }
}