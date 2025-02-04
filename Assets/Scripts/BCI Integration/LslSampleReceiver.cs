using LSL;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LslSampleReceiver: MonoBehaviour
{
    [SerializeField] string _streamName = "PythonResponse";
    [SerializeField] bool _autoStart = true;

    [Header("Timing Settings")]
    [SerializeField] float _resolverPeriod = 0.1f;
    [SerializeField] float _openStreamTimeout = 1;
    [SerializeField] float _pullSampleTimeout = 1;

    public bool HasResolvedStream => _resolvedStreamInfo != null;
    private ContinuousResolver _resolver;
    private StreamInfo _resolvedStreamInfo;
    
    private StreamInlet _inlet;
    private string[] _sampleBuffer;


    private void Start()
    {
        if (_autoStart) FindAndConnectToStream();
    }

    private void OnDestroy()
    {
        CloseInlet();
    }


    public void FindAndConnectToStream()
    {
        if (string.IsNullOrEmpty(_streamName))
        {
            Debug.LogWarning("Can't search for a nameless stream, aborting...");
            return;
        }
        _resolver = new("name", _streamName);
        StartCoroutine(RunResolveStream());
    }

    IEnumerator RunResolveStream()
    {
        StreamInfo[] resolvedStreams = _resolver.results();

        while (resolvedStreams.Length == 0)
        {
            yield return new WaitForSeconds(_resolverPeriod);
            resolvedStreams = _resolver.results();
        }

        _resolvedStreamInfo = resolvedStreams[0];
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

    public void OpenInlet(StreamInfo streamInfo)
    {
        _sampleBuffer = new string[streamInfo.channel_count()];
        _inlet = new StreamInlet(streamInfo);
        _inlet.open_stream(_openStreamTimeout);
    }

    public void CloseInlet()
    {
        _inlet?.Dispose();
        _inlet = null;
    }
    

    public LslSample[] PullAllSamples()
    {
        if (_inlet == null) return new LslSample[0];

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