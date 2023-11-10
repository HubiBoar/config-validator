using ModulR.Validation.Abstraction;
using OneOf;
using OneOf.Types;

namespace ConfigValidator.Contracts;

public sealed record MethodNotFoundError;

public interface IMappingValidator
{
    OneOf<Success, MethodNotFoundError, ValidationErrors> Validate(string value, Mapping mapping);
}