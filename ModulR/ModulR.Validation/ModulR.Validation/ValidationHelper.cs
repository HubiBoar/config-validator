using ModulR.Validation.Abstraction;

namespace ModulR.Validation;

public static class ValidationHelper
{
    public static IReadOnlyCollection<OneOf<Success, ValidationErrors>> ValidateCollection<TMethod, TValue>(
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidationMethod<TValue>
    {
        return collection.Select(TMethod.Validate).ToArray();
    }
}