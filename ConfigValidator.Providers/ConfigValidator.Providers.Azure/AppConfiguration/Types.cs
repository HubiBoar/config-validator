using ModulR.Extensions.FluentValidation.External;
using ModulR.ValueWrapper;

namespace ConfigValidator.Providers.Azure.AppConfiguration;

public sealed record AppConfigNoAccess(Exception Exception);

public sealed record AppConfigName(string Name) : ValueRecord<IsNotEmpty>(Name);

public sealed record AppConfigurationConnectionString(string Value) : ValueRecord<IsConnectionString>(Value);