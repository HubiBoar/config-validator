using ModulR.Extensions.FluentValidation.External;

namespace ModulR.Extensions.FluentValidation;

public interface IFluentValidatable<TSelf> : IFluentValidatableSetup<TSelf>, IValidatable
    where TSelf : IFluentValidatable<TSelf>
{
    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return FluentValidationHelper.ValidateFluently((TSelf)this, TSelf.SetupValidation);
    }
}

public interface IFluentValidatableParams<TSelf> : IFluentValidatable<TSelf>
    where TSelf : IFluentValidatableParams<TSelf>
{
    static void IFluentValidatableSetup<TSelf>.SetupValidation(FluentValidator<TSelf> fluentValidator)
    {
        //fluentValidator has parameters validation already in constructor
    }
}

public interface IFluentValidatableSetup<TSelf>
    where TSelf : IFluentValidatableSetup<TSelf>
{
    public static abstract void SetupValidation(FluentValidator<TSelf> fluentValidator);
}