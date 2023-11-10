using Azure.Data.AppConfiguration;
using Azure.Security.KeyVault.Secrets;
using ConfigValidator.Console;
using ConfigValidator.Contracts;
using OneOf;

namespace ConfigValidator.Providers.Azure.AppConfiguration;

internal record ConfigurationValue(OneOf<string, ValueNotFound> Value);

internal class AzureAppConfigurationProvider : IValueForKeyProvider
{
    private readonly ConfigurationClient _configurationClient;
    private readonly SecretClient _keyVaultClient;

    internal AzureAppConfigurationProvider(ConfigurationClient configurationClient, SecretClient keyVaultClient)
    {
        _configurationClient = configurationClient;
        _keyVaultClient = keyVaultClient;
    }

    public OneOf<string, ValueNotFound> GetValue(KeyName keyName)
    {
        return GetConfigurationValue(keyName, _configurationClient, _keyVaultClient).Value;
    }
    
    private static ConfigurationValue GetConfigurationValue(
        KeyName keyName,
        ConfigurationClient client,
        SecretClient secretClient)
    {
        return GetConfigSetting(keyName, client).Match<ConfigurationValue>(config =>
            config.IsKeyVaultReference().Match(reference =>
            {
                try
                {
                    var value = secretClient.GetSecret(reference.Name);

                    return new ConfigurationValue(value.HasValue ? value.Value.Value : new ValueNotFound());
                }
                catch
                {
                    return new ConfigurationValue(new ValueNotFound());
                }
            },
            notReference => new ConfigurationValue(config.Setting.Value)),
        notFound => new ConfigurationValue(notFound));
    }

    private static OneOf<AppConfigurationConfig, ValueNotFound> GetConfigSetting(
        KeyName keyName,
        ConfigurationClient client)
    {
        try
        {
            //Label should be at the beginning, separated by dot, example: Test.SqlConnectionString: IsConnectionString
            var split = keyName.Name.Split(".");

            var label = split.Length > 1 ? split[0] : null;
            var name = split.Length > 1 ? split[1] : keyName.Name;
            
            var setting = client.GetConfigurationSetting(name, label);

            return new AppConfigurationConfig(setting);
        }
        catch
        {
            return new ValueNotFound();
        }
    }
}