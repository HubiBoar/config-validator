using OneOf;

namespace ConfigValidator.Contracts;

public sealed record ValueNotFound;

public interface IValueForKeyProvider
{
    OneOf<string, ValueNotFound> GetValue(KeyName keyName);
}