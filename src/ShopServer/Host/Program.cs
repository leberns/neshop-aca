using AiClient.AzureChat;
using AiClient.AzureEmbedding;
using AiClient.Interfaces;
using AiClient.Products;
using Contracts.Products.Repositories;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.Repositories;
using Core.Products;
using Database;
using Database.DataSeed;
using Host.Api;
using Host.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppMonitoring(builder.Configuration);

using var loggerFactory = LoggerFactory.Create(loggerBuilder => loggerBuilder.AddConsole());
var logger = loggerFactory.CreateLogger(nameof(Program));

var postgresDataSource = PostgresDataSource.Configure(builder.Configuration, logger);

builder.Services
    .AddAppDbContext(postgresDataSource)
    .AddSeeders()
    .AddAppAzureOpenAiClient(builder.Configuration, logger)
    .AddScoped<IChatService, AzureChatService>()
    .AddScoped<ITextEmbedder, AzureTextEmbedder>()
    .AddScoped<IProductEmbedder, ProductEmbedder>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IProductRepositoryAiSearch, ProductRepositoryAiSearch>()
    .AddScoped<IProductsReader, ProductsReader>()
    .AddScoped<IProductRagService, ProductRagService>()
    .AddScoped<IProductsAiSearchService, ProductsAiSearchService>();

builder.Services
    .AddAuthorization()
    .AddOpenApi()
    .AddHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.SeedProductEmbeddingsAsync(logger);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "NeShop API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapProductEndpoints();
app.MapProductSearchEndpoints();

app.MapHealthChecks("/health");

app.Run();
