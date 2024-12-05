using UnityEngine;

public class CharacterPresenter: MonoBehaviour
{
    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();

        BattleEventBus.ActiveBlockStarted += SetIdleState;
        BattleEventBus.IdleBlockStarted += SetIdleState;
        BattleEventBus.WindupStarted += PlayWindupAnimation;
        BattleEventBus.SphereThrown += PlayThrowAnimation;
    }

    private void OnDestroy()
    {
        BattleEventBus.ActiveBlockStarted -= SetIdleState;
        BattleEventBus.IdleBlockStarted -= SetIdleState;
        BattleEventBus.WindupStarted -= PlayWindupAnimation;
        BattleEventBus.SphereThrown -= PlayThrowAnimation;
    }


    private void SetIdleState() => _animator.SetBool("Active", BlockManager.IsActiveBlock);
    private void PlayWindupAnimation() => _animator.SetTrigger("Charge");
    private void PlayThrowAnimation() =>_animator.SetTrigger("Throw");
}