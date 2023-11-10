using ModulR.Extensions.FluentValidation.External;
using ModulR.ValueWrapper;

namespace ConfigValidator.Providers.Azure;

public sealed record TenantId(string Value) : ValueRecord<IsNotEmpty>(Value);

public sealed record ClientId(string Value) : ValueRecord<IsNotEmpty>(Value);

public sealed record ClientSecret(string Value) : ValueRecord<IsNotEmpty>(Value);