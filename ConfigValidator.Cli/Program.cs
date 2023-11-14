using ConfigValidator.Cli;
using ConfigValidator.Providers.Azure;
using ModulR.Extensions.Cocona;

await CoconaModularStartup.Setup(
    builder =>
    {
        builder.AddModule(new AzureAppConfigModule(ConsoleRunner.Run));
    },
    host =>
    {

    });