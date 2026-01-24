using Azure.Identity;
using Contracts;
using Contracts.Options;
using Npgsql;

namespace Host.Extensions;

public static class PostgresDataSourceExtensions
{
    public static NpgsqlDataSource? ConfigurePostgresDataSource(
        this WebApplicationBuilder builder,
        ILogger logger)
    {
        var connectionString = builder.Configuration.GetConnectionString(Constants.ConnectionStringNames.ShopDatabase);
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var hasConnectionString = !string.IsNullOrWhiteSpace(connectionString);
        var hasPassword = !string.IsNullOrWhiteSpace(dataSourceBuilder.ConnectionStringBuilder.Password);

        switch (hasConnectionString)
        {
            case true when !hasPassword:
            {
                // When running with Azure username in the cloud or locally, there is no password. The managed identity is used to get a token.
                logger.LogInformation("Using managed identity to fetch database token");

                var options = builder.Configuration.GetSection(nameof(ManagedIdentityOptions)).Get<ManagedIdentityOptions>()
                    ?? throw new InvalidOperationException($"{nameof(ManagedIdentityOptions)} are not configured");

                var tokenCredential = new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        ManagedIdentityClientId = options.ManagedIdentityClientId
                    });

                dataSourceBuilder.UsePeriodicPasswordProvider(async (_, cancellationToken) =>
                {
                    var requestContext = new Azure.Core.TokenRequestContext([Constants.Identity.DatabaseTokenScope]);
                    var accessToken = await tokenCredential.GetTokenAsync(requestContext, cancellationToken);

                    logger.LogInformation("Database token expiration on {expires}", accessToken.ExpiresOn.ToString("O"));

                    return accessToken.Token;
                }, TimeSpan.FromHours(1), TimeSpan.FromSeconds(5));

                return dataSourceBuilder.Build();
            }

            case true when hasPassword:
                // When migrating or running locally connected to the local database, there is a password in the connection string.
                // Assume no need to refresh tokens if a password was given, even if eventually a token was passed instead of a fixed password.
                logger.LogInformation("Using password authentication to access the database");
                return dataSourceBuilder.Build();

            default:
                // When creating the migration in container, there is no connection string (hasConnectionString is false in Dockerfile).
                logger.LogInformation("No connection string was provided to access the database");
                return null;
        }
    }
}