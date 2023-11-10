using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ModulR.Validation;
using OneOf;
using OneOf.Types;

namespace ConfigValidator.Providers.Azure.AppConfiguration;

internal sealed record KeyVaultReference(string Name);

internal sealed  record AppConfigurationConfig(ConfigurationSetting Setting)
{
    public OneOf<KeyVaultReference, False> IsKeyVaultReference()
    {
        if (Setting.Value.Contains("""{"uri":"""))
        {
            return new KeyVaultReference(Setting.Value.Split("/").Last().TrimEnd('}', '"'));
        }

        return new False();
    }

    public bool IsFeatureFlag => Setting.Key.Contains(".appconfig.featureflag/");
}

internal class AppConfigurationHelper
{
    public static OneOf<ConfigurationClient, AppConfigNoAccess> GetAppConfigurationClient(string appConfigName)
    {
        if(new AppConfigName(appConfigName).IsValid().TryPickT1(out _, out var configName))
        {
            return new AppConfigNoAccess(new Exception("AppConfigName is invalid"));
        }

        return GetAppConfigurationClient(configName);
    }
    
    public static OneOf<ConfigurationClient, AppConfigNoAccess> GetAppConfigurationClientConnectionString(
        string appConfigurationConnectionString)
    {
        if(new AppConfigurationConnectionString(appConfigurationConnectionString)
           .IsValid()
           .TryPickT1(out _, out var connectionString))
        {
            return new AppConfigNoAccess(new Exception("AppConfigurationConnectionString is invalid"));
        }

        return GetAppConfigurationClient(connectionString);
    }
    
    public static OneOf<ConfigurationClient, AppConfigNoAccess> GetAppConfigurationClientSecret(
        string secretName,
        SecretClient secretClient)
    {
        try
        {
            var result = secretClient.GetSecret(secretName);
            
            if(new AppConfigurationConnectionString(result.Value.Value)
               .IsValid()
               .TryPickT1(out _, out var connectionString))
            {
                return new AppConfigNoAccess(new Exception("AppConfigurationConnectionString is invalid"));
            }

            return GetAppConfigurationClient(connectionString);
        }
        catch (Exception exception)
        {
            return new AppConfigNoAccess(new Exception("Secret not found", exception));
        }
    }
    
    private static OneOf<ConfigurationClient, AppConfigNoAccess> GetAppConfigurationClient(
        Valid<AppConfigurationConnectionString> appConfigurationConnectionString)
    {        
        var client = new ConfigurationClient(appConfigurationConnectionString.ValidValue.Value);
        return TestAppConfigurationClient(client);
    }
    
    private static OneOf<ConfigurationClient, AppConfigNoAccess> GetAppConfigurationClient(
        Valid<AppConfigName> appConfigName)
    {
        var appConfigUrl = $"https://{appConfigName.ValidValue.Value}.azconfig.io";
        var credential = new DefaultAzureCredential();

        var client = new ConfigurationClient(new Uri(appConfigUrl), credential);
        return TestAppConfigurationClient(client);
    }

    private static OneOf<ConfigurationClient, AppConfigNoAccess> TestAppConfigurationClient(
        ConfigurationClient client)
    {
        try
        {
            client.GetConfigurationSetting("test");

            return client;
        }
        catch (RequestFailedException failed) when (failed.Status == 404)
        {
            return client;
        }
        catch (Exception exception)
        {
            return new AppConfigNoAccess(exception);
        }
    }
}