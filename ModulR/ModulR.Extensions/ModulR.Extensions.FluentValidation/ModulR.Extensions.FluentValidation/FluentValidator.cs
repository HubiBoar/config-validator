namespace ModulR.Extensions.FluentValidation;

public sealed class FluentValidator<T> : AbstractValidator<T>
{
    internal FluentValidator()
    {
        var validatableType = typeof(T);
        
        var validatableProperties = validatableType
            .GetProperties()
            .Where(x => x.PropertyType.IsAssignableTo(typeof(IValidatable)))
            .ToArray();
    
        foreach (var property in validatableProperties)
        {
            RuleFor(x => (IValidatable)property.GetValue(x)!).Custom((v, context) =>
            {
                var errors = v.IsValid().Match<IReadOnlyCollection<string>>(
                    _ => Array.Empty<string>(),
                    errors => errors.ValidationErrors.ErrorMessages);

                foreach (var error in errors)
                {
                    context.AddFailure(new ValidationFailure(property.Name, error));
                }
            });
            //MemberValidateSelf(property);
        }
    }
    
    // private void MemberValidateSelf(PropertyInfo property)
    // {
    //     //x => x.{property.Name}
    //     var param = Expression.Parameter(typeof(T), "x");
    //
    //     var member = Expression.MakeMemberAccess(param, property);
    //
    //     var rule = Expression.Lambda<Func<T, IValidatable>>(member, param);
    //
    //     RuleFor(rule).ValidateSelf();
    // }
}