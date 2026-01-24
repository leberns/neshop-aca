using AiClient.Services;
using Contracts.Embedding.Services;
using Contracts.Products.Repositories;
using Contracts.Products.Services;
using Contracts.ProductsAiSearch.Repositories;
using Contracts.ProductsAiSearch.Services;
using Core.Products;
using Core.ProductsAiSearch;
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
    .AddAppAiClient(builder.Configuration, logger)
    .AddScoped<IEmbedder, AppAzureOpenAiEmbedder>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IProductRepositoryAiSearch, ProductRepositoryAiSearch>()
    .AddScoped<IProductsReader, ProductsReader>()
    .AddScoped<IProductRagService, ProductRagService>()
    .AddScoped<IProductsSearchService, ProductsSearchService>();

builder.Services
    .AddAuthorization()
    .AddOpenApi()
    .AddHealthChecks();

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
app.MapProductSearchEndpoints();

app.MapHealthChecks("/health");

app.Run();
