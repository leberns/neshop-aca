using Azure.AI.OpenAI;
using Azure.Identity;
using Contracts.Options;

namespace Host.Extensions;

public static class AddAppAiClientExtensions
{
    public static IServiceCollection AddAppAzureOpenAiClient(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger)
    {
        services
            .AddOptions<AiOptions>()
            .BindConfiguration(nameof(AiOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(_ =>
        {
            var aiOptions = configuration.GetSection(nameof(AiOptions)).Get<AiOptions>()
                            ?? throw new InvalidOperationException(
                                $"{nameof(AiOptions)} are missing from configuration.");

            var managedIdentityOptions =
                configuration.GetSection(nameof(ManagedIdentityOptions)).Get<ManagedIdentityOptions>()
                ?? throw new InvalidOperationException(
                    $"{nameof(ManagedIdentityOptions)} are missing from configuration.");

            if (!string.IsNullOrWhiteSpace(aiOptions.ApiKey))
            {
                logger.LogInformation("Using API key to connect to AI provider");
                return new AzureOpenAIClient(
                    new Uri(aiOptions.Endpoint),
                    new System.ClientModel.ApiKeyCredential(aiOptions.ApiKey));
            }

            logger.LogInformation("Using managed identity to connect to AI provider");
            var credential = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = managedIdentityOptions.ManagedIdentityClientId
                });

            return new AzureOpenAIClient(new Uri(aiOptions.Endpoint), credential);
        });

        return services;
    }
}