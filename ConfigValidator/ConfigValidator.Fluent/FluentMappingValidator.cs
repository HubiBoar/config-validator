using ConfigValidator.Contracts;
using ModulR.Extensions.FluentValidation;
using ModulR.Extensions.FluentValidation.External;
using ModulR.Validation.Abstraction;
using OneOf;
using OneOf.Types;

namespace ConfigValidator.Fluent;

internal sealed class FluentMappingValidator : IMappingValidator, IFluentMappingValidatorOptions
{
    public abstract class Method
    {
        public abstract OneOf<Success, ValidationErrors> Validate(string value, KeyName keyName);
    }
    
    private sealed class FluentMethod<TMethod> : Method
        where TMethod : IFluentValidationMethod<string>
    {
        public override OneOf<Success, ValidationErrors> Validate(string value, KeyName keyName)
        {
            return FluentValidationHelper.Validate<TMethod, string>(value, keyName.Name);
        }
    }
    
    private sealed class Method<TMethod> : Method
        where TMethod : IValidationMethod<string>
    {
        public override OneOf<Success, ValidationErrors> Validate(string value, KeyName keyName)
        {
            return TMethod.Validate(value);
        }
    }

    private readonly Dictionary<MethodName, Method> _validationMethods;

    public FluentMappingValidator()
    {
        _validationMethods = new Dictionary<MethodName, Method>();
    }

    public IFluentMappingValidatorOptions Register<TMethod>(string name)
        where TMethod : IFluentValidationMethod<string>
    {
        _validationMethods.TryAdd(new MethodName(name), new FluentMethod<TMethod>());

        return this;
    }
    
    public IFluentMappingValidatorOptions RegisterCustom<TMethod>(string name)
        where TMethod : IValidationMethod<string>
    {
        _validationMethods.TryAdd(new MethodName(name), new Method<TMethod>());

        return this;
    }
    
    public OneOf<Success, MethodNotFoundError, ValidationErrors> Validate(string value, Mapping mapping)
    {
        return TryGetMethod(mapping.MethodName)
            .Match<OneOf<Success, MethodNotFoundError, ValidationErrors>>(
                validator => validator
                    .Validate(value, mapping.KeyName)
                    .Match<OneOf<Success, MethodNotFoundError, ValidationErrors>>(
                        success => success,
                        error => error),
                error => error);
    }
    
    private OneOf<Method, MethodNotFoundError> TryGetMethod(MethodName methodName)
    {
        if (_validationMethods.TryGetValue(methodName, out var validator))
        {
            return OneOf<Method, MethodNotFoundError>.FromT0(validator);
        }

        return new MethodNotFoundError();
    }
    
    // public void RegisterFromAssembly(Assembly assembly)
    // {
    //     var types = assembly
    //         .GetTypes()
    //         .Where(x => x.IsAssignableTo(typeof(IValidationMethod<TValue>)))
    //         .ToArray();
    //
    //     foreach (var type in types)
    //     {
    //         GetType()
    //             .GetMethod(nameof(Register), BindingFlags.Instance | BindingFlags.Public)!
    //             .MakeGenericMethod(type)
    //             .Invoke(this, Array.Empty<object>());
    //     }
    // }
}
