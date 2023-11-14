using Azure.Security.KeyVault.Secrets;
using Cocona;
using Cocona.Builder;
using ConfigValidator.Console;

namespace ConfigValidator.Providers.Azure.KeyVault;

internal static class KeyVaultCommands
{
    public static void AddCommands(ICoconaCommandsBuilder app, Action<CallBuilder> finishedCallBack)
    {
        app.AddCommand("KeyVault", (
            DefaultParameters defaultParameters,
            KeyVaultParameters keyVaultParameters) =>
        {
            KeyVaultHelper.GetKeyVaultClient(keyVaultParameters).Switch(
                secret => Run(secret, defaultParameters, finishedCallBack),
                Throw);
        });
    }
    
    private static void Run(
        SecretClient keyVaultClient,
        DefaultParameters defaultParameters,
        Action<CallBuilder> finishedCallBack)
    {
        var valueProvider = new AzureKeyVaultProvider(keyVaultClient);
        finishedCallBack(new CallBuilder(defaultParameters, valueProvider));
    }

    private static void Throw(KeyVaultNoAccess noAccess)
    {
        throw new Exception("KeyVaultNoAccess", noAccess.Exception);
    }
}