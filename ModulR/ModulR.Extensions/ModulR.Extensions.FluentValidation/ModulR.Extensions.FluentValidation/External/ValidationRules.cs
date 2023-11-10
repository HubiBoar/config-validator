namespace ModulR.Extensions.FluentValidation.External;

public static class ValidationRules
{
    public static void UseValidationMethod<TMethod, T>(this IRuleBuilder<T, string> rule)
        where TMethod : IFluentValidationMethod<string>
        where T : IValidatable
    {
        TMethod.SetupValidation<T>(rule);
    }

    public static void UseValidationMethod<TMethod, T, TValue>(this IRuleBuilder<T, TValue> rule)
        where TMethod : IFluentValidationMethod<TValue>
        where T : IValidatable
    {
        TMethod.SetupValidation<T>(rule);
    }
    
    public static void IsConnectionString<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsClientId<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsCron<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(2);
    }

    public static void IsUrl<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsKey<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsSendGridTemplateId<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsDomainName<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsContainerName<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsSecret<T>(this IRuleBuilder<T, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }
}