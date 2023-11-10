using ConfigValidator.Contracts;
using OneOf;

namespace ConfigValidatorRunner;

internal class TestValueForKeyProvider : IValueForKeyProvider
{
    public OneOf<string, ValueNotFound> GetValue(KeyName keyName)
    {
        return keyName.Name;
    }
}