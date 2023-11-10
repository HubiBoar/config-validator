using MoreLinq.Extensions;

namespace ModulR.Extensions.FluentValidation.External;

public static class FluentValidationHelper
{
    public static OneOf<Success, ValidationErrors> ValidateFluently<T>(T value, Action<FluentValidator<T>> validationSetup)
    {
        var fluentValidator = new FluentValidator<T>();
        validationSetup(fluentValidator);

        return fluentValidator.Validate(value).ToResult();
    }
    
    public static OneOf<Success, ValidationErrors> ValidateFluentlyWithRule<T>(
        T value,
        Action<IRuleBuilder<T, T>> validationSetup)
    {
        return ValidateFluentlyWithRule(value, "Value", validationSetup);
    }
    
    public static OneOf<Success, ValidationErrors> ValidateFluentlyWithRule<T>(
        T value,
        string valueName,
        Action<IRuleBuilder<T, T>> validationSetup)
    {
        var fluentValidator = new FluentValidator<T>();

        var rule = fluentValidator.RuleFor(v => v);
        
        validationSetup(rule);

        rule.Must(x => true).WithName(valueName);

        return fluentValidator.Validate(value).ToResult();
    }

    public static OneOf<Success, ValidationErrors> Validate<TMethod, TValue>(TValue value, string valueName)
        where TMethod : IFluentValidationMethod<TValue>
    {
        return ValidateFluentlyWithRule(value, valueName, TMethod.SetupValidation);
    }
    
    public static OneOf<Success, ValidationErrors> Validate<TMethod, TValue>(TValue value)
        where TMethod : IFluentValidationMethod<TValue>
    {
        return ValidateFluentlyWithRule(value, TMethod.SetupValidation);
    }

    public static void ValidateCollection<TMethod, TValue, TContextType>(
        IReadOnlyCollection<TValue> collection,
        ValidationContext<TContextType> context)
        where TMethod : IFluentValidationMethod<TValue>
    {
        collection.ForEach((property, propertyIndex) =>
        {
            var errors = TMethod.Validate(property).Match<IReadOnlyCollection<string>>(
                success => Array.Empty<string>(),
                errors => errors.ErrorMessages);

            errors.ForEach(error => context.AddFailure(new ValidationFailure($"[{propertyIndex}]", error)));
        });
    }
}