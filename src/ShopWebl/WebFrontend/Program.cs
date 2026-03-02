using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using ShopWeb.ShopClient;
using WebContracts;
using WebContracts.Options;
using WebCore;
using WebFrontend.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLocalization()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHealthChecks(); // make the "/health" endpoint available

builder.Services.AddOptions<ShopClientOptions>()
    .BindConfiguration("ShopClient")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<IShopClient, ShopClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<ShopClientOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services
    .AddScoped<IProductsService, ProductsService>()
    .AddScoped<IProductsSearch, ProductsSearch>();

var app = builder.Build();

string[] supportedCultures = ["en-US", "en-CH", "de-CH", "fr-CH", "it-CH"];

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

StaticWebAssetsLoader.UseStaticWebAssets(
    builder.Environment,
    builder.Configuration);

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,
});

app.Run();
