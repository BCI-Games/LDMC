using UnityEngine;

public class CharacterPresenter: MonoBehaviour
{
    protected Animator _animator;


    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();

        BattleEventBus.OnBlockStarted += SetBlockState;
        BattleEventBus.OffBlockStarted += SetBlockState;
        BattleEventBus.WindupStarted += PlayWindupAnimation;
        BattleEventBus.WindupCancelled += CancelWindupAnimation;
        BattleEventBus.SphereThrown += PlayReleaseAnimation;
    }

    protected virtual void OnDestroy()
    {
        BattleEventBus.OnBlockStarted -= SetBlockState;
        BattleEventBus.OffBlockStarted -= SetBlockState;
        BattleEventBus.WindupStarted -= PlayWindupAnimation;
        BattleEventBus.WindupCancelled -= CancelWindupAnimation;
        BattleEventBus.SphereThrown -= PlayReleaseAnimation;
    }


    protected virtual void SetBlockState() => _animator.SetBool("Active", BlockManager.IsOnBlock);
    protected void PlayWindupAnimation() => _animator.SetBool("Charge", true);
    protected void CancelWindupAnimation() => _animator.SetBool("Charge", false);
    protected void PlayReleaseAnimation()
    {
        _animator.SetTrigger("Release");
        _animator.SetBool("Charge", false);
    }
}