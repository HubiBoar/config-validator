namespace ModulR.ValueWrapper;

internal interface IValue<TSelf, TMethod, TValue> : IValidatable
    where TMethod : IValidationMethod<TValue>
    where TSelf : IValue<TSelf, TMethod, TValue>
{
    public TValue GetValue();

    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return TMethod.Validate(GetValue());
    }

    public static abstract implicit operator TSelf(TValue value);

    public static abstract implicit operator TValue(TSelf self);
}

public class Value<TMethod, TValue> : IValue<Value<TMethod, TValue>, TMethod, TValue>
    where TMethod : IValidationMethod<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue() => _value;

    public override string? ToString() => GetValue()?.ToString();
    
    public static implicit operator Value<TMethod, TValue>(TValue value)
    {
        return new Value<TMethod, TValue>(value);
    }

    public static implicit operator TValue(Value<TMethod, TValue> self)
    {
        return self._value;
    }
}

public class Value<TMethod> : IValue<Value<TMethod>, TMethod, string>
    where TMethod : IValidationMethod<string>
{
    private readonly string _value;

    private Value(string value)
    {
        _value = value;
    }

    public string GetValue() => _value;

    public override string ToString() => GetValue();

    public static implicit operator Value<TMethod>(string value)
    {
        return new Value<TMethod>(value);
    }

    public static implicit operator string(Value<TMethod> self)
    {
        return self._value;
    }
}

public record ValueRecord<TMethod>(string Value) : IValue<ValueRecord<TMethod>, TMethod, string>
    where TMethod : IValidationMethod<string>
{
    public string GetValue() => Value;
    
    public override string ToString() => GetValue();

    public static implicit operator ValueRecord<TMethod>(string value)
    {
        return new ValueRecord<TMethod>(value);
    }

    public static implicit operator string(ValueRecord<TMethod> self)
    {
        return self.Value;
    }
}

public record ValueRecord<TMethod, TValue>(TValue Value) : IValue<ValueRecord<TMethod, TValue>, TMethod, TValue>
    where TMethod : IValidationMethod<TValue>
{
    public TValue GetValue() => Value;

    public override string? ToString() => GetValue()?.ToString();

    public static implicit operator ValueRecord<TMethod, TValue>(TValue value)
    {
        return new ValueRecord<TMethod, TValue>(value);
    }

    public static implicit operator TValue(ValueRecord<TMethod, TValue> self)
    {
        return self.Value;
    }
}