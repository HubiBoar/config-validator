using Cocona;

namespace ConfigValidator.Providers.Azure.KeyVault;

public class KeyVaultParameters : ICommandParameterSet
{
    [Option(Description = "Name of AzureKeyVault to connect to.")]
    public string KeyVaultName { get; set; } = string.Empty;

    [Option(Description = "TenantId used to connect to AzureKeyVault.")]
    [HasDefaultValue]
    public string? TenantId { get; set; } = null!;

    [Option(Description = "ClientId used to connect to AzureKeyVault.")]
    [HasDefaultValue]
    public string? ClientId { get; set; } = null!;
    
    [Option(Description = "ClientSecret used to connect to AzureKeyVault.")]
    [HasDefaultValue]
    public string? ClientSecret { get; set; } = null!;
}