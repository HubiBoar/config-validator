using ConfigValidator.Contracts;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigValidator.Yaml;

internal class MappingsFromYamlFileProvider : IMappingProvider
{
    private readonly FilePath _path;

    public MappingsFromYamlFileProvider(FilePath path)
    {
        _path = path;
    }

    public IReadOnlyCollection<Mapping> GetMapping()
    {
        var yml = File.ReadAllText(_path.Path);

        return new MappingsFromYamlProvider(new Yaml(yml)).GetMapping();
    }
}