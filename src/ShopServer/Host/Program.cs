using Azure.Identity;
using Contracts;
using Contracts.Options;
using Contracts.Services;
using Database;
using Database.DataSeed;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using AiClient.Services;
using Core.ReadProduct;
using Core.SemanticSearchProduct;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(loggerBuilder => loggerBuilder.AddConsole());
var logger = loggerFactory.CreateLogger(nameof(Program));

var postgresDataSource = ConfigurePostgresDataSource(builder, logger);

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    if (postgresDataSource is null)
    {
        options.UseNpgsql(string.Empty);
        return;
    }

    options.UseNpgsql(postgresDataSource);
});

builder.Services.AddOptions<AiOptions>()
    .BindConfiguration(nameof(AiOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddSeeders()
    .AddScoped<ShopRepository>()
    .AddScoped<IEmbedder, AzureOpenAiEmbedder>()
    .AddScoped<IProductsReader, ProductsReader>()
    .AddScoped<IProductRagService, ProductRagService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // map https://localhost:5051/openapi/v1.json

    app.UseSwaggerUI(options => // enable https://localhost:5051/swagger
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Shop API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

return;

static NpgsqlDataSource? ConfigurePostgresDataSource(
    WebApplicationBuilder builder,
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