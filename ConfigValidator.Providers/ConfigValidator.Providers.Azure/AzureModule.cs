using Cocona;
using Cocona.Builder;
using ConfigValidator.Console;
using ConfigValidator.Providers.Azure.AppConfiguration;
using ConfigValidator.Providers.Azure.KeyVault;

namespace ConfigValidator.Providers.Azure;

public class AzureAppConfigModule : IModule
{
    public static void Configure(ICoconaCommandsBuilder app, Action<CallBuilder> callback)
    {
        app.AddSubCommand("Azure", x =>
        {
            x.AddModule<AppConfigurationModule>(callback);
            
            x.AddModule<KeyVaultModule>(callback);
        });
    }
}