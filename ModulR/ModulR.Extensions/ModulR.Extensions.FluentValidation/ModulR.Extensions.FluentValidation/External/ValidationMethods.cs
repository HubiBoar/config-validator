using System.Text.Json;

namespace ModulR.Extensions.FluentValidation.External;

public sealed class IsConnectionString : IFluentValidationMethod<IsConnectionString, string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().MinimumLength(4);
    }
}

public sealed class IsEmail : IFluentValidationMethod<IsEmail, string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.EmailAddress();
    }
}

public sealed class IsUrl : IFluentValidationMethod<IsUrl, string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.IsUrl();
    }
}

public sealed class IsJsonArrayOf<TMethod> : IFluentValidationMethod<IsJsonArrayOf<TMethod>, string>
    where TMethod : IFluentValidationMethod<string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.Custom((array, context) =>
        {
            //Convert property to json
            var properties = JsonSerializer.Deserialize<IReadOnlyCollection<string>>(array);
            
            if (properties is null)
            {
                context.AddFailure(new ValidationFailure("JsonArray", "Could not deserialize"));
                return;
            }

            FluentValidationHelper.ValidateCollection<TMethod, string, T>(properties, context);
        });
    }
}

public sealed class IsCommaArrayOf<TMethod> : IFluentValidationMethod<IsCommaArrayOf<TMethod>, string>
    where TMethod : IFluentValidationMethod<string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.Custom((array, context) =>
        {
            var properties = array.Split(",");

            if (properties.Length == 0)
            {
                return;
            }

            FluentValidationHelper.ValidateCollection<TMethod, string, T>(properties, context);
        });
    }
}

public sealed class IsNotEmpty : IFluentValidationMethod<IsNotEmpty, string>
{
    public static void SetupValidation<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().NotNull();
    }
}