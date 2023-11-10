using ModulR.Validation;
using ModulR.Validation.Abstraction;
using OneOf;
using OneOf.Types;

namespace ConfigValidator.Contracts;

public sealed record ValidMappingResult(
    Valid<Mapping> Mapping,
    OneOf<Success, ValueNotFound, ValidationErrors, MethodNotFoundError> Result);

public sealed record ValidationResult(OneOf<ValidMappingResult, InValid<Mapping>> Result);

public static class Validator
{
    public static IReadOnlyCollection<ValidationResult> Validate(
        IMappingProvider mappingProvider,
        IValueForKeyProvider valueForKeyProvider,
        IMappingValidator validator,
        Action<ValidationResult> callBack)
    {
        return mappingProvider
            .GetMapping()
            .Select(mapping =>
            {
                var result = mapping.IsValid().Match<ValidationResult>(validMapping =>
                {
                    var validMappingResult = valueForKeyProvider.GetValue(mapping.KeyName).Match<ValidMappingResult>(
                        value =>
                            validator.Validate(value, mapping).Match<ValidMappingResult>(
                                success => new ValidMappingResult(validMapping, success),
                                methodNotFound => new ValidMappingResult(validMapping, methodNotFound),
                                validationError => new ValidMappingResult(validMapping, validationError)),
                        valueNotFound => new ValidMappingResult(validMapping, valueNotFound));

                    return new ValidationResult(validMappingResult);

                }, inValidMapping => new ValidationResult(inValidMapping));

                callBack(result);

                return result;
            })
            .ToArray();
    }
}