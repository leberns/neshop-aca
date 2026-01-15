using Microsoft.Extensions.DependencyInjection;

namespace Database.DataSeed;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        services.AddTransient<ISeeder, BrandSeeder>();
        services.AddTransient<ISeeder, CategorySeeder>();
        services.AddTransient<ISeeder, ProductSeeder>();
        services.AddTransient<ISeeder, ImageSeeder>();
        services.AddTransient<ISeeder, CustomerSeeder>();
        services.AddTransient<ISeeder, CartSeeder>();
        services.AddTransient<ISeeder, ProductCartSeeder>();
        services.AddTransient<ISeeder, ReviewSeeder>();

        return services;
    }
}