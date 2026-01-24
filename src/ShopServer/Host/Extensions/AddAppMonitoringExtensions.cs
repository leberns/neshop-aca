using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Contracts;
using Contracts.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Host.Extensions;

public static class AddAppMonitoringExtensions
{
    public static void AddAppMonitoring(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .UseAzureMonitor(o =>
            {
                // note: set the environment variable APPLICATIONINSIGHTS_CONNECTION_STRING with the connection string to the Application Insights resource

                var managedIdentityOptions = configuration.GetSection(nameof(ManagedIdentityOptions)).Get<ManagedIdentityOptions>();

                if(string.IsNullOrWhiteSpace(managedIdentityOptions?.ManagedIdentityClientId))
                {
                    o.Credential = new DefaultAzureCredential(); // for development use the default credential
                }
                else
                {
                    o.Credential = new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            ManagedIdentityClientId = managedIdentityOptions.ManagedIdentityClientId
                        });
                }
            });
    }

    private static void ConfigureResource(ResourceBuilder r)
    {
        r.AddService(Constants.Metadata.AppName,
            serviceVersion: Constants.Metadata.AppVersion,
            serviceInstanceId: Environment.MachineName);
    }
}