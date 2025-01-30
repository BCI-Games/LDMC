using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatedCharacterPresenter: CharacterPresenter
{
    [SerializeField] protected Animator _sleepyZedAnimator;

    private Animator _animator;


    protected override void Start()
    {
        if (Settings.AnimationSimplified)
        {
            Destroy(gameObject);
            return;
        }
        _animator = GetComponent<Animator>();

        SubscribeToBattleEvents();
    }


    protected override void ShowWindupStarted() => _animator.SetBool("Charge", true);
    protected override void ShowWindupCancelled() => _animator.SetBool("Charge", false);
    protected override void ShowThrow()
    {
        _animator.SetTrigger("Release");
        _animator.SetBool("Charge", false);
    }

    protected override void ShowRestEnded()
    {
        _animator.SetBool("Resting", false);
        _animator.ResetTrigger("Snooze");
        _sleepyZedAnimator.gameObject.SetActive(false);
    }

    protected override void ShowRestStarted()
    {
        _animator.SetBool("Resting", true);
        _sleepyZedAnimator.gameObject.SetActive(true);
        EmitSleepyZed();
    }

    protected override IEnumerator RunRestCycle()
    {
        yield return new WaitForSeconds(Settings.AnimationCycleDuration);
        _animator.SetTrigger("Snooze");
        EmitSleepyZed();
    }
    
    protected void EmitSleepyZed() => _sleepyZedAnimator.SetTrigger("Emit");
}