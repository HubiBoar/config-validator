using ModulR.Extensions.FluentValidation.External;
using ModulR.ValueWrapper;

namespace ConfigValidator.Providers.Azure.KeyVault;

public sealed record KeyVaultName(string Name) : ValueRecord<IsNotEmpty>(Name);

public sealed record KeyVaultNoAccess(Exception Exception);