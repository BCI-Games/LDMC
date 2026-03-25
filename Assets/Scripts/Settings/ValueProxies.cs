using System;

public class FloatProxy : ValueProxy<float>
{
    public FloatProxy(float defaultValue) => _value = defaultValue;
    public override ParsingMethod Parse => float.Parse;
    public override AttemptedParsingMethod TryParse => float.TryParse;
}

public class IntProxy : ValueProxy<int>
{
    public IntProxy(int defaultValue) => _value = defaultValue;
    public override ParsingMethod Parse => int.Parse;
    public override AttemptedParsingMethod TryParse => int.TryParse;
}

public class BooleanProxy : ValueProxy<bool>
{
    public BooleanProxy(bool defaultValue) => _value = defaultValue;
    public override ParsingMethod Parse => bool.Parse;
    public override AttemptedParsingMethod TryParse => bool.TryParse;
}

public class ExclusiveBooleanProxy : BooleanProxy
{
    protected BooleanProxy _condition;
    public ExclusiveBooleanProxy(bool defaultValue, BooleanProxy condition)
    : base(defaultValue) => _condition = condition;

    public override bool GetValue() => _value && !_condition;
    public bool GetRawValue() => _value;
}

public abstract class ValueProxy<T> : ValueProxy
{
    public event Action Modified;
    protected T _value;

    public virtual T GetValue() => _value;
    public void SetValue(T value)
    {
        if (_value.Equals(value)) return;
        _value = value;
        Modified?.Invoke();
        InstanceModified?.Invoke();
    }
    public void SetValue(string valueString) => SetValue(Parse(valueString));
    public bool TrySetValue(string valueString)
    {
        if (TryParse(valueString, out T parsedValue))
        {
            SetValue(parsedValue);
            return true;
        }
        return false;
    }


    public delegate T ParsingMethod(string valueString);
    public abstract ParsingMethod Parse { get; }

    public delegate bool AttemptedParsingMethod(string valueString, out T output);
    public abstract AttemptedParsingMethod TryParse { get; }

    public static implicit operator T(ValueProxy<T> proxy) => proxy.GetValue();
}
public abstract class ValueProxy
{
    public static Action InstanceModified;
}