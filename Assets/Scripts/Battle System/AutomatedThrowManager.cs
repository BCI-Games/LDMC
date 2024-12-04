using UnityEngine;
using System.Collections;

public class AutomatedThrowManager: ThrowManager
{
    [SerializeField] private bool _autoPlay = true;
    [SerializeField] private float _throwDelay = 0.5f;

    private bool _isThrowing;
    private Coroutine _autoThrowCoroutine;


    protected override void Start()
    {
        base.Start();
        if (_autoPlay) StartAutoThrow();
    }

    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            if(_isThrowing) StopAutoThrow(); else StartAutoThrow();
    }


    public void StartAutoThrow()
    {
        StopAutoThrow();
        _autoThrowCoroutine = StartCoroutine(RunAutoThrow());
    }

    public void StopAutoThrow()
    {
        if (_isThrowing)
        {
            StopCoroutine(_autoThrowCoroutine);
            _isThrowing = false;
            ChargeLevel = 0;
        }
    }

    private IEnumerator RunAutoThrow()
    {
        _isThrowing = true;
        while(_numberOfSpheresRemaining > 0)
        {
            ThrowSphere();
            yield return new WaitForSeconds(_throwDelay);
            ChargeLevel = 1;
            yield return new WaitForSeconds(_chargePeriod);
        }
        _isThrowing = false;
    }
}