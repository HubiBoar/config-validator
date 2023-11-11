using ConfigValidator.Contracts;
using OneOf;

namespace ConfigValidator.Cli;

internal class TestValueForKeyProvider : IValueForKeyProvider
{
    public OneOf<string, ValueNotFound> GetValue(KeyName keyName)
    {
        return keyName.Name;
    }
}