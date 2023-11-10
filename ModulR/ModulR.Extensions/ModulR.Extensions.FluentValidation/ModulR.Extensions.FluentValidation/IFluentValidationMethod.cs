using ModulR.Extensions.FluentValidation.External;

namespace ModulR.Extensions.FluentValidation;

public interface IFluentValidationMethod<TSelf, in TValue> : IFluentValidationMethod<TValue>
    where TSelf : IFluentValidationMethod<TSelf, TValue>
{
    static OneOf<Success, ValidationErrors> IValidationMethod<TValue>.Validate(TValue value)
    {
        return FluentValidationHelper.Validate<TSelf, TValue>(value);    
    }
}

public interface IFluentValidationMethod<in TValue> : IValidationMethod<TValue>
{
    public static abstract void SetupValidation<T>(IRuleBuilder<T, TValue> ruleBuilder);
}