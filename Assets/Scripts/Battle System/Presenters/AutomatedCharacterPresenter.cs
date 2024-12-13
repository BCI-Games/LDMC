using UnityEngine;
using System.Collections;

public class AutomatedCharacterPresenter: CharacterPresenter
{
    protected override void SetBlockState()
    {
        base.SetBlockState();
        if (!BlockManager.IsOnBlock)
            StartRestCycle();
    }

    private void StartRestCycle()
    {
        _sleepyZedAnimator.gameObject.SetActive(true);
        EmitSleepyZed();
        StartCoroutine(RunRestCycle());
    }

    private IEnumerator RunRestCycle()
    {
        while(!BlockManager.IsOnBlock)
        {
            yield return new WaitForSeconds(Settings.AnimationCycleDuration);
            _animator.SetTrigger("Snooze");
            EmitSleepyZed();
        }
        _animator.ResetTrigger("Snooze");
        _sleepyZedAnimator.gameObject.SetActive(false);
    }
}