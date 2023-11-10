using Azure.Data.AppConfiguration;
using Azure.Security.KeyVault.Secrets;
using Cocona;
using Cocona.Builder;
using ConfigValidator.Console;
using ConfigValidator.Providers.Azure.KeyVault;

namespace ConfigValidator.Providers.Azure.AppConfiguration;

internal class AppConfigurationModule : IModule
{
    public static void Configure(ICoconaCommandsBuilder app, Action<CallBuilder> finishedCallBack)
    {
        app.AddCommand("AppConfiguration", (
            DefaultParameters defaultParameters,
            KeyVaultParameters keyVaultParameters,
            [Option(Description = "AppConfig Name.")] string appConfigName) =>
        {
            KeyVaultHelper.GetKeyVaultClient(keyVaultParameters).Switch(
                secret =>
                    AppConfigurationHelper.GetAppConfigurationClient(appConfigName).Switch(config =>
                        Run(config, secret, defaultParameters, finishedCallBack),
                        Throw),
                Throw);
        });
        
        app.AddCommand("AppConfigurationConnectionString", (
            DefaultParameters defaultParameters,
            KeyVaultParameters keyVaultParameters,
            [Option(Description = "AppConfig ConnectionString.")] string connectionString) =>
        {
            KeyVaultHelper.GetKeyVaultClient(keyVaultParameters).Switch(
                secret =>
                    AppConfigurationHelper.GetAppConfigurationClientConnectionString(connectionString).Switch(config =>
                        Run(config, secret, defaultParameters, finishedCallBack),
                        Throw),
                Throw);
        });
        
        app.AddCommand("AppConfigurationSecret", (
            DefaultParameters defaultParameters,
            KeyVaultParameters keyVaultParameters,
            [Option(Description = "AppConfig Secret in KeyVault containing ConnectionString.")] string secretName) =>
        {
            KeyVaultHelper.GetKeyVaultClient(keyVaultParameters).Switch(
                secret =>
                    AppConfigurationHelper.GetAppConfigurationClientSecret(secretName, secret).Switch(config =>
                        Run(config, secret, defaultParameters, finishedCallBack),
                        Throw),
                Throw);
        });
    }

    private static void Run(
        ConfigurationClient configurationClient,
        SecretClient keyVaultClient,
        DefaultParameters defaultParameters,
        Action<CallBuilder> finishedCallBack)
    {
        var valueProvider = new AzureAppConfigurationProvider(configurationClient, keyVaultClient);
        finishedCallBack(new CallBuilder(defaultParameters, valueProvider));
    }

    private static void Throw(KeyVaultNoAccess noAccess)
    {
        throw new Exception("KeyVaultNoAccess", noAccess.Exception);
    }
    
    private static void Throw(AppConfigNoAccess noAccess)
    {
        throw new Exception("AppConfigNoAccess", noAccess.Exception);
    }
}