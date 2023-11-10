using Cocona;
using ConfigValidator.Console;
using ConfigValidator.Contracts;
using ConfigValidator.Fluent;
using ConfigValidator.Presentation;
using ConfigValidator.Providers.Azure;
using ConfigValidator.Yaml;
using ConfigValidatorRunner;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

AddModule<AzureAppConfigModule>(app);

app.Run();

static void AddModule<TModule>(CoconaApp app)
    where TModule : IModule
{
    app.AddModule<TModule>(Run);
}

static void Run(CallBuilder builder)
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

    Console.WriteLine(resultDrawing.Value);

    if (resultDrawing.Status.IsT1)
    {
        throw new Exception();
    }
}