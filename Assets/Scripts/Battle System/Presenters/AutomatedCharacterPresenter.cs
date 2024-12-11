using UnityEngine;
using System.Collections;

public class AutomatedCharacterPresenter: CharacterPresenter
{
    private float _idleDuration;
    private float _activeDuration;

    private Coroutine _restCycleCoroutine;
    private bool _isRunningRestCycle;


    protected override void Start()
    {
        base.Start();
        Settings.AddAndInvokeModificationCallback(UpdateTimings);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Settings.Modified -= UpdateTimings;
    }


    private void UpdateTimings()
    {
        _idleDuration = Settings.CharacterIdleDuration;
        _activeDuration = Settings.CharacterActiveDuration;
    }


    protected override void SetBlockState()
    {
        base.SetBlockState();
        if (!BlockManager.IsOnBlock)
            StartRestCycle();
        else
            StopRestCycle();
    }

    private void StartRestCycle()
    {
        StopRestCycle();
        _restCycleCoroutine = StartCoroutine(RunRestCycle());
    }

    private void StopRestCycle()
    {
        if (_isRunningRestCycle)
        {
            StopCoroutine(_restCycleCoroutine);
            _isRunningRestCycle = false;
            _animator.ResetTrigger("Release");
        }
    }

    private IEnumerator RunRestCycle()
    {
        _isRunningRestCycle = true;
        while(!BlockManager.IsOnBlock)
        {
            yield return new WaitForSeconds(_idleDuration);
            PlayWindupAnimation();
            yield return new WaitForSeconds(_activeDuration);
            PlayReleaseAnimation();
        }
        _isRunningRestCycle = false;
    }
}