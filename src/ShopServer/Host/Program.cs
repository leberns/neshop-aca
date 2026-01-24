using Contracts.Products.Repositories;
using Contracts.Products.Services;
using Core.Products;
using Database;
using Database.DataSeed;
using Host.Api;
using Host.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppMonitoring(builder.Configuration);

using var loggerFactory = LoggerFactory.Create(loggerBuilder => loggerBuilder.AddConsole());
var logger = loggerFactory.CreateLogger(nameof(Program));

builder.Services.AddOptions<AiOptions>()
    .BindConfiguration(nameof(AiOptions))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(_ =>
{
    var aiOptions = builder.Configuration.GetSection(nameof(AiOptions)).Get<AiOptions>()
                    ?? throw new InvalidOperationException($"{nameof(AiOptions)} are missing from configuration.");

    var managedIdentityOptions = builder.Configuration.GetSection(nameof(ManagedIdentityOptions)).Get<ManagedIdentityOptions>()
                                 ?? throw new InvalidOperationException($"{nameof(ManagedIdentityOptions)} are missing from configuration.");

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

var postgresDataSource = builder.ConfigurePostgresDataSource(logger);

builder.Services
    .AddAppDbContext(postgresDataSource)
    .AddSeeders()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IEmbedder, AzureOpenAiEmbedder>()
    .AddScoped<IProductsReader, ProductsReader>()
    .AddScoped<IProductRagService, ProductRagService>()
    .AddScoped<IProductsSearchService, ProductsSearchService>();

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Shop API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapProductEndpoints();

app.MapHealthChecks("/health");

app.Run();
