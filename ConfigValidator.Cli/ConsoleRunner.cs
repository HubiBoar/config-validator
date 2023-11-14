using ConfigValidator.Console;
using ConfigValidator.Contracts;
using ConfigValidator.Fluent;
using ConfigValidator.Presentation;
using ConfigValidator.Yaml;

namespace ConfigValidator.Cli;

public static class ConsoleRunner
{
    public static void Run(CallBuilder builder)
    {
        var resultDrawer = ValidationResultDrawerFactory.Create();
        var mappingValidator = FluentMappingValidatorFactory.Create(ValidationOptions.RegisterMethods);
        var mappingsProvider = MappingsFromYamlProviderFactory.FromFile(builder.FilePath);
        var valueProvider = builder.ValueForKeyProviderProvider;

        var validationResult = Validator.Validate(
            mappingsProvider,
            valueProvider,
            mappingValidator,
            _ => { });

        var resultDrawing = resultDrawer.DrawResults(validationResult);

        System.Console.WriteLine(resultDrawing.Value);

        if (resultDrawing.Status.IsT1)
        {
            throw new Exception();
        }
    }
}