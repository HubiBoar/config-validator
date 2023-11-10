using Azure.Data.AppConfiguration;
using Azure.Security.KeyVault.Secrets;
using ConfigValidator.Console;
using ConfigValidator.Contracts;
using OneOf;

namespace ConfigValidator.Providers.Azure.AppConfiguration;

internal class AzureKeyVaultProvider : IValueForKeyProvider
{
    private readonly SecretClient _keyVaultClient;

    internal AzureKeyVaultProvider(SecretClient keyVaultClient)
    {
        _keyVaultClient = keyVaultClient;
    }

    public OneOf<string, ValueNotFound> GetValue(KeyName keyName)
    {
        return GetConfigurationValue(keyName, _keyVaultClient).Value;
    }
    
    private static ConfigurationValue GetConfigurationValue(
        KeyName keyName,
        SecretClient secretClient)
    {
        try
        {
            var value = secretClient.GetSecret(keyName.Name);

            return new ConfigurationValue(value.HasValue ? value.Value.Value : new ValueNotFound());
        }
        catch
        {
            return new ConfigurationValue(new ValueNotFound());
        }
    }
}