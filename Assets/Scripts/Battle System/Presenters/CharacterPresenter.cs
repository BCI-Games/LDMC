using UnityEngine;

public class CharacterPresenter: MonoBehaviour
{
    [SerializeField] protected Animator _sleepyZedAnimator;

    protected Animator _animator;


    protected virtual void Start()
    {
        if (Settings.AnimationSimplified)
        {
            Destroy(gameObject);
            return;
        }

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

    protected void EmitSleepyZed() => _sleepyZedAnimator.SetTrigger("Emit");
}