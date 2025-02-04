using System.Text.RegularExpressions;

public class LslSample
{
    public readonly double CaptureTime;
    public readonly string[] RawValue;

    public LslSample(double captureTime, string[] value)
    {
        CaptureTime = captureTime;
        RawValue = value;
    }

    public static LslSample Parse(string[] value, double captureTime)
    {
        if (value.Length == 1)
        {
            string message = value[0];
            if (string.IsNullOrEmpty(message))
                return new EmptyLslSample(captureTime);

            if (TestRegex(message, @"^ping$"))
                return new LslPing(captureTime, value);

            string capturedString;
            if (TestRegex(message, @"^\[?(\d+)\]?$", out capturedString))
                return new LslIntegerSample(captureTime, value, capturedString);

            if (TestRegex(message, @"^marker received : (.+)$", out capturedString))
                return new LslMarkerSample(captureTime, value, capturedString);
        }
        return new LslSample(captureTime, value);
    }

    private static bool TestRegex(string input, string pattern)
    {
        return Regex.Match(input, pattern).Success;
    }

    private static bool TestRegex(string input, string pattern, out string matchingGroup)
    {
        Match match = Regex.Match(input, pattern);
        if (match.Success)
        {
            matchingGroup = match.Groups[1].Value;
            return true;
        }
        matchingGroup = "";
        return false;
    }
}

public class LslPing: LslSample
{
    public LslPing
        (double captureTime, string[] value)
        : base(captureTime, value) {}
}

public class EmptyLslSample: LslSample
{
    public EmptyLslSample
        (double captureTime)
        : base(captureTime, null) {}
}

public abstract class LslSample<T>: LslSample
{
    public readonly T Value;

    public LslSample
    (
        double captureTime,
        string[] rawValue,
        T parsedValue
    )
    : base(captureTime, rawValue)
    {
        Value = parsedValue;
    }
}


public class LslIntegerSample: LslSample<int>
{
    public LslIntegerSample
    (
        double captureTime,
        string[] rawValue,
        string integerString
    )
    : base(captureTime, rawValue, int.Parse(integerString))
    {}
}

public class LslMarkerSample: LslSample<string>
{
    public LslMarkerSample
    (
        double captureTime,
        string[] rawValue,
        string message
    )
    : base(captureTime, rawValue, message)
    {}
}