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

var postgresDataSource = builder.ConfigurePostgresDataSource(logger);

builder.Services
    .AddAppDbContext(postgresDataSource)
    .AddSeeders()
    .AddScoped<ProductsRepository>()
    .AddScoped<IProductsReader, ProductsReader>();

builder.Services.AddAuthorization();
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

app.MapProductEndpoints();

app.MapHealthChecks("/health");

app.Run();
