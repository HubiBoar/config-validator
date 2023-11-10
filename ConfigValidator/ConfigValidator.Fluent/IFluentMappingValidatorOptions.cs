using ModulR.Extensions.FluentValidation;
using ModulR.Validation.Abstraction;

namespace ConfigValidator.Fluent;

public interface IFluentMappingValidatorOptions
{
    public IFluentMappingValidatorOptions Register<TMethod>(string name)
        where TMethod : IFluentValidationMethod<string>;

    public IFluentMappingValidatorOptions RegisterCustom<TMethod>(string name)
        where TMethod : IValidationMethod<string>;
}