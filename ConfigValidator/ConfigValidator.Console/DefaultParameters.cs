using Cocona;

namespace ConfigValidator.Console;

public class DefaultParameters : ICommandParameterSet
{
    [Option('f', Description = "Path to Yaml validation configuration file.")]
    public string FilePath { get; set; } = string.Empty;
}