using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Infrastructure.Persistence;
using CarLog.Maintenance.Infrastructure.Repositories;

namespace CarLog.Maintenance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment) 
    {
        var connectionString = configuration.GetConnectionString("AzureSqlConnection") ?? throw new InvalidOperationException("Connection string 'AzureSqlConnection' is not set.");

        services.AddDbContext<MaintenanceDbContext>((sp, options) => 
        {
            options.UseSqlServer(connectionString, sqlOptions => 
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

                sqlOptions.MigrationsAssembly(typeof(MaintenanceDbContext).Assembly.FullName);

                sqlOptions.CommandTimeout(30);
            });

            if (environment.IsDevelopment() && configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks().AddDbContextCheck<MaintenanceDbContext>(name: "maintenance-sql-db", tags: new[] { "database", "sql", "azure" });

        return services;
    }
}
