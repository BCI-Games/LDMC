using LSL;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BciEssentialsInputManager: MonoBehaviour
{
    [SerializeField] string _streamName = "PythonResponse";
    [SerializeField] float _resolverPeriod = 0.1f;
    [SerializeField] float _openStreamTimeout = 1;
    [SerializeField] float _pullSampleTimeout = 1;

    private ContinuousResolver _resolver;

    private StreamInlet _inlet;
    private string[] _sampleBuffer;


    private void Start()
    {
        _resolver = new("name", _streamName);
        StartCoroutine(ResolveExpectedStream());
    }

    private void OnDestroy()
    {
        _inlet?.Dispose();
        _inlet = null;
    }

    IEnumerator ResolveExpectedStream()
    {
        StreamInfo[] resolvedStreams = _resolver.results();

        while (resolvedStreams.Length == 0)
        {
            yield return new WaitForSeconds(_resolverPeriod);
            resolvedStreams = _resolver.results();
        }

        StreamInfo streamInfo = resolvedStreams[0];
        _sampleBuffer = new string[streamInfo.channel_count()];

        _inlet = new StreamInlet(streamInfo);
        _inlet.open_stream(_openStreamTimeout);
    }
    
    void Update()
    {
        if (_inlet == null) return;

        double lastCaptureTime = double.MaxValue;

        int i = 0;
        while (lastCaptureTime > 0 && i++ < 10)
        {
            lastCaptureTime = _inlet.pull_sample(_sampleBuffer, _pullSampleTimeout);

            if (lastCaptureTime != 0 && SampleBufferContainsValues())
            {
                string sampleMessage = _sampleBuffer[0];

                if (sampleMessage.Equals("ping"))
                {
                    _inlet.Dispose();
                    _inlet = null;
                    return;
                }

                Match match = Regex.Match(sampleMessage, @"^\[(?<value>\d+)\]");
                if (match.Success)
                    Debug.Log("Prediction Marker Received: " + match.Groups["value"]);
            }
        }
    }

    private bool SampleBufferContainsValues()
    {
        foreach (string s in _sampleBuffer)
        {
            if (!string.IsNullOrEmpty(s))
                return true;
        }
        return false;
    }
}