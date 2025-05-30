using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimatedCharacterPresenter : CharacterPresenter
{
    [SerializeField] protected Animator _sleepyZedAnimator;
    public bool CelebrationEnabled = false;
    [SerializeField] float _celebrationPeriod = 0.5f;

    private Animator _animator;
    private readonly List<MonsterData> _discoveredMonsters = new();
    private bool _currentMonsterIsNew;


    protected override void Start()
    {
        if (Settings.AnimationSimplified)
        {
            Destroy(gameObject);
            return;
        }
        _animator = GetComponent<Animator>();

        SubscribeToBattleEvents();
        BattleEventBus.MonsterAppeared += CheckMonsterNovelty;
    }

    protected override void OnDestroy()
    {
        UnsubscribeFromBattleEvents();
        BattleEventBus.MonsterAppeared -= CheckMonsterNovelty;
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
        if (CelebrationEnabled && _currentMonsterIsNew)
        {
            StartCoroutine(RunCelebrationDisplay());
        }
        else StartRestAnimation();
    }

    private void StartRestAnimation()
    {
        _animator.SetBool("Resting", true);
        _animator.ResetTrigger("Snooze");
        _sleepyZedAnimator.gameObject.SetActive(true);
        EmitSleepyZed();
    }

    private IEnumerator RunCelebrationDisplay()
    {
        _animator.Play("Celebrate");
        yield return new WaitForSeconds(_celebrationPeriod);
        StartRestAnimation();
    }

    protected override IEnumerator RunRestCycle()
    {
        yield return new WaitForSeconds(Settings.AnimationCycleDuration);
        _animator.SetTrigger("Snooze");
        EmitSleepyZed();
    }

    protected void EmitSleepyZed() => _sleepyZedAnimator.SetTrigger("Emit");
    
    
    private void CheckMonsterNovelty(MonsterData newMonster)
    {
        _currentMonsterIsNew = !_discoveredMonsters.Contains(newMonster);
        if (_currentMonsterIsNew) _discoveredMonsters.Add(newMonster);
    }
}