using ConfigValidator.Fluent;
using ModulR.Extensions.FluentValidation.External;

namespace ConfigValidator.Cli;

public static class ValidationOptions
{
    public static void RegisterMethods(IFluentMappingValidatorOptions options)
    {      
        options
            .Register<IsConnectionString>("IsConnectionString")
            .Register<IsUrl>("IsUrl")
            .Register<IsEmail>("IsEmail");
    }
}