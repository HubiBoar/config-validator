using ConfigValidator.Contracts;
using OneOf;
using OneOf.Types;

namespace ConfigValidator.Presentation;

public interface IValidationResultDrawer
{
    public sealed record Result(string Value, OneOf<Success, Error> Status);

    public Result DrawResults(IReadOnlyCollection<ValidationResult> validationResults);
}