using ConfigValidator.Contracts;

namespace ConfigValidator.Fluent;

public static class FluentMappingValidatorFactory
{
    public static IMappingValidator Create(Action<IFluentMappingValidatorOptions> options)
    {
        var validator = new FluentMappingValidator();

        options(validator);

        return validator;
    }
}