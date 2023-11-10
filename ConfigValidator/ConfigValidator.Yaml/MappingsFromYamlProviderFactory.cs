using ConfigValidator.Contracts;

namespace ConfigValidator.Yaml;

public sealed record FilePath(string Path);

public sealed record Yaml(string Value);

public static class MappingsFromYamlProviderFactory
{
    public static IMappingProvider FromFile(FilePath path)
    {
        return new MappingsFromYamlFileProvider(path);
    }
    
    public static IMappingProvider Direct(Yaml yml)
    {
        return new MappingsFromYamlProvider(yml);
    }
}