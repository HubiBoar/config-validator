using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ModulR.Validation;
using OneOf;

namespace ConfigValidator.Providers.Azure.KeyVault;

internal static class KeyVaultHelper
{
    public static OneOf<SecretClient, KeyVaultNoAccess> GetKeyVaultClient(KeyVaultParameters parameters)
    {
        if(new KeyVaultName(parameters.KeyVaultName).IsValid().TryPickT1(out _, out var keyVaultName))
        {
            return new KeyVaultNoAccess(new Exception("keyVaultName is invalid"));
        }

        if (parameters.TenantId is null || parameters.ClientId is null || parameters.ClientSecret is null)
        {
            return GetKeyVaultClient(keyVaultName);
        }

        if(new TenantId(parameters.TenantId).IsValid().TryPickT1(out _, out var tenantId))
        {
            return new KeyVaultNoAccess(new Exception("TenantId is invalid"));
        }

        if(new ClientId(parameters.ClientId).IsValid().TryPickT1(out _, out var clientId))
        {
            return new KeyVaultNoAccess(new Exception("ClientId is invalid"));
        }

        if(new ClientSecret(parameters.ClientSecret).IsValid().TryPickT1(out _, out var clientSecret))
        {
            return new KeyVaultNoAccess(new Exception("ClientSecret is invalid"));
        }

        return GetKeyVaultClient(keyVaultName, tenantId, clientId, clientSecret);
    }

    private static OneOf<SecretClient, KeyVaultNoAccess> GetKeyVaultClient(
        Valid<KeyVaultName> keyVaultName,
        Valid<TenantId> tenant,
        Valid<ClientId> clientId,
        Valid<ClientSecret> clientSecret)
    {
        var keyVaultUrl = $"https://{keyVaultName.ValidValue.Name}.vault.azure.net/";

        var credential = new ClientSecretCredential(tenant.ValidValue, clientId.ValidValue, clientSecret.ValidValue);

        var client = new SecretClient(new Uri(keyVaultUrl), credential);
        return TestKeyVaultClient(client);
    }
    
    private static OneOf<SecretClient, KeyVaultNoAccess> GetKeyVaultClient(
        Valid<KeyVaultName> keyVaultName)
    {
        var keyVaultUrl = $"https://{keyVaultName.ValidValue.Name}.vault.azure.net/";

        var credential = new DefaultAzureCredential();

        var client = new SecretClient(new Uri(keyVaultUrl), credential);
        return TestKeyVaultClient(client);
    }

    private static OneOf<SecretClient, KeyVaultNoAccess> TestKeyVaultClient(SecretClient client)
    {
        try
        {
            client.GetSecret("test");

            return client;
        }
        catch (RequestFailedException failed) when (failed.Status == 404)
        {
            return client;
        }
        catch (Exception exception)
        {
            return new KeyVaultNoAccess(exception);
        }
    }
}