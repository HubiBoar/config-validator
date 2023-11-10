using ModulR.Extensions.FluentValidation;
using ModulR.Extensions.FluentValidation.External;
using ModulR.Validation;
using ModulR.Validation.Abstraction;
using ModulR.ValueWrapper;
using OneOf;

namespace ConfigValidator.Contracts;

public sealed record KeyName(string Name) : ValueRecord<IsNotEmpty>(Name);

public sealed record MethodName(string Name) : ValueRecord<IsNotEmpty>(Name);

public sealed record Mapping(KeyName KeyName, MethodName MethodName) : IFluentValidatableParams<Mapping>;

public interface IMappingProvider
{
    public IReadOnlyCollection<Mapping> GetMapping();
}