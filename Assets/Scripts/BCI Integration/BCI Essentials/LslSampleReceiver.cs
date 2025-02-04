using LSL;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LslSampleReceiver: MonoBehaviour
{
    [SerializeField] string _streamName = "PythonResponse";
    [SerializeField] bool _autoResolve = false;

    [Header("Timing Settings")]
    [Min(0)]
    [SerializeField] float _nameResolutionPeriod = 0.1f;
    [SerializeField] float _openStreamTimeout = 0;
    [Tooltip("A non-zero value will significantly impact performance")]
    [SerializeField] float _pullSampleTimeout = 0;

    protected bool IsResolvingStream = false;
    private ContinuousResolver _resolver;
    protected bool HasResolvedStream => _resolvedStreamInfo != null;
    private StreamInfo _resolvedStreamInfo;
    
    protected bool HasLiveInlet => _inlet != null;
    private StreamInlet _inlet;
    private string[] _sampleBuffer;


    private void Start()
    {
        if (_pullSampleTimeout > 0)
        {
            Debug.LogWarning("Pulling Timeout is non-zero, this will block the main thread and significantly impact performance.");
        }
        if (_autoResolve) FindAndConnectToStream();
    }

    private void OnDestroy()
    {
        CloseInlet();
    }


    public void FindAndConnectToStream()
    {
        if (HasResolvedStream)
        {
            OpenInletForResolvedStream();
            return;
        }

        if (string.IsNullOrEmpty(_streamName))
        {
            Debug.LogWarning("Can't search for a nameless stream, aborting...");
            return;
        }
        _resolver = new("name", _streamName);
        StartCoroutine(RunResolveStreamByName());
    }

    IEnumerator RunResolveStreamByName()
    {
        IsResolvingStream = true;
        StreamInfo[] resolvedStreams = _resolver.results();

        while (resolvedStreams.Length == 0)
        {
            yield return new WaitForSeconds(_nameResolutionPeriod);
            resolvedStreams = _resolver.results();
        }

        _resolvedStreamInfo = resolvedStreams[0];
        IsResolvingStream = false;
        OpenInletForResolvedStream();
    }


    public void OpenInletForResolvedStream()
    {
        if (!HasResolvedStream)
        {
            Debug.LogWarning("No stream resolved, aborting...");
            return;
        }
        OpenInlet(_resolvedStreamInfo);
    }

    protected void OpenInlet(StreamInfo streamInfo)
    {
        _sampleBuffer = new string[streamInfo.channel_count()];
        _inlet = new StreamInlet(streamInfo);
        _inlet.open_stream(_openStreamTimeout);
    }

    protected void CloseInlet()
    {
        _inlet?.Dispose();
        _inlet = null;
    }
    

    public LslSample[] PullAllSamples()
    {
        if (!HasLiveInlet) return new LslSample[0];

        List<LslSample> pulledSamples = new();
        double lastCaptureTime = double.MaxValue;
        int loopCounter = 0;

        while (lastCaptureTime > 0 && loopCounter++ < 10)
        {
            LslSample sample;
            lastCaptureTime = PullSample(out sample);
            if (lastCaptureTime > 0 && !(sample is EmptyLslSample))
                pulledSamples.Add(sample);
        }
        return pulledSamples.ToArray();
    }

    private double PullSample(out LslSample parsedSample)
    {
        double captureTime = _inlet.pull_sample(_sampleBuffer, _pullSampleTimeout);
        parsedSample = LslSample.Parse(_sampleBuffer, captureTime);
        return captureTime;
    }
}