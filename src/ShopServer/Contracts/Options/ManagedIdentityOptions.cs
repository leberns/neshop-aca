using System.ComponentModel.DataAnnotations;

namespace Contracts.Options;

public record ManagedIdentityOptions
{
    /// <summary>
    /// The client id of the managed identity used by the app to access Azure resources like the database.
    /// See in Azure Portal, Managed identity of the app, copy the client id, and use it in the app configuration.
    /// </summary>
    [Required]
    public string ManagedIdentityClientId { get; set; } = string.Empty;
}