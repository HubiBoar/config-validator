using ConfigValidator;
using ConfigValidator.Fluent;
using Microsoft.Extensions.Configuration;
using ModulR.Extensions.FluentValidation.External;

namespace ConfigValidatorRunner;

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