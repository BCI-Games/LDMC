using UnityEngine;

public class CharacterPresenter: MonoBehaviour
{
    private Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();

        BattleEventBus.OnBlockStarted += SetBlockState;
        BattleEventBus.OffBlockStarted += SetBlockState;
        BattleEventBus.WindupStarted += PlayWindupAnimation;
        BattleEventBus.SphereThrown += PlayThrowAnimation;
    }

    private void OnDestroy()
    {
        BattleEventBus.OnBlockStarted -= SetBlockState;
        BattleEventBus.OffBlockStarted -= SetBlockState;
        BattleEventBus.WindupStarted -= PlayWindupAnimation;
        BattleEventBus.SphereThrown -= PlayThrowAnimation;
    }


    private void SetBlockState() => _animator.SetBool("Active", BlockManager.IsOnBlock);
    private void PlayWindupAnimation() => _animator.SetTrigger("Charge");
    private void PlayThrowAnimation() =>_animator.SetTrigger("Throw");
}