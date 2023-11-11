using Azure.Security.KeyVault.Secrets;
using ConfigValidator.Contracts;
using ConfigValidator.Providers.Azure.AppConfiguration;
using OneOf;

namespace ConfigValidator.Providers.Azure.KeyVault;

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