using Cocona;
using ConfigValidator.Console;
using ConfigValidator.Providers.Azure.AppConfiguration;
using ConfigValidator.Providers.Azure.KeyVault;
using ModulR.Extensions.Cocona;
using ModulR.Startup;

namespace ConfigValidator.Providers.Azure;

public sealed class AzureAppConfigModule : ICoconaModule
{
    private readonly Action<CallBuilder> _finishedCallBack;

    public AzureAppConfigModule(Action<CallBuilder> finishedCallBack)
    {
        _finishedCallBack = finishedCallBack;
    }

    public void SetupModule(ModuleSetup setup)
    {
    }

    public void SetupHost(HostSetup<CoconaApp> setup)
    {
        setup.Host.AddSubCommand("Azure", x =>
        {
            KeyVaultCommands.AddCommands(x, _finishedCallBack);
            AppConfigurationCommands.AddCommands(x, _finishedCallBack);
        });
    }
}