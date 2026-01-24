using Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Host.Extensions;

public static class AppDbContextExtensions
{
    public static IServiceCollection AddAppDbContext(
        this IServiceCollection services,
        NpgsqlDataSource? postgresDataSource = null)
    {
        services.AddDbContext<ShopDbContext>(options =>
        {
            if (postgresDataSource is null)
            {
                options.UseNpgsql(string.Empty);
                return;
            }

            options.UseNpgsql(postgresDataSource);
        });

        return services;
    }
}