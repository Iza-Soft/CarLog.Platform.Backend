using CarLog.Vehicle.Application.Common.Interfaces;
using CarLog.Vehicle.Application.Interfaces;
using CarLog.Vehicle.Infrastructure.Persistence;
using CarLog.Vehicle.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarLog.Vehicle.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("AzureSqlConnection") ?? throw new InvalidOperationException("Connection string not set");

        services.AddDbContext<VehicleDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                sqlOptions.MigrationsAssembly(typeof(VehicleDbContext).Assembly.FullName);
                sqlOptions.CommandTimeout(30);
            });

            if (environment.IsDevelopment() && configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks().AddDbContextCheck<VehicleDbContext>(name: "vehicle-sql-db", tags: new[] { "database", "sql", "azure" });

        return services;
    }
}