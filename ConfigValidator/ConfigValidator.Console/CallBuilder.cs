using ConfigValidator.Contracts;
using ConfigValidator.Yaml;

namespace ConfigValidator.Console;

public sealed class CallBuilder
{
    public FilePath FilePath { get; }

    public IValueForKeyProvider ValueForKeyProviderProvider { get; }

    public CallBuilder(DefaultParameters parameters, IValueForKeyProvider valueForKeyProviderProvider)
    {
        FilePath = new FilePath(parameters.FilePath);
        ValueForKeyProviderProvider = valueForKeyProviderProvider;
    }
}