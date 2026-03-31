using System;
using System.Collections;
using BCIEssentials;
using BCIEssentials.LSLFramework;
using UnityEngine;


public class RestingStateConductor : MonoBehaviour, IMarkerSource
{
    public MarkerWriter MarkerWriter { get; set; }
    public static event Action CollectionStarted;
    public static event Action CollectionEnded;

    [SerializeField] private GameObject _overlay;


    private void OnDestroy() => StopAllCoroutines();
    public void StartCollection()
    {
        _overlay.SetActive(false);
        StartCoroutine(RunRestingStateCollection());
    }


    private IEnumerator RunRestingStateCollection()
    {
        float previousTimeScale = Time.timeScale;
        Time.timeScale = 1;

        CollectionStarted?.Invoke();
        yield return new WaitForSeconds(Settings.RestingStateDuration);
        MarkerWriter.PushDoneWithRestingStateCollectionMarker();
        CollectionEnded?.Invoke();

        Time.timeScale = previousTimeScale;
        _overlay.SetActive(true);
    }
}