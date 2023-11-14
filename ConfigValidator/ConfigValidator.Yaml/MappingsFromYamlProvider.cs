using ConfigValidator.Contracts;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigValidator.Yaml;

internal class MappingsFromYamlProvider : IMappingProvider
{
    private readonly Yaml _yml;

    public MappingsFromYamlProvider(Yaml yml)
    {
        _yml = yml;
    }

    public IReadOnlyCollection<Mapping> GetMapping()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var result = deserializer.Deserialize<Dictionary<string, string>>(_yml.Value);
        
        return result.Select(x =>
                new Mapping(
                    new KeyName(x.Key),
                    new MethodName(x.Value)))
            .ToArray();
    }
}