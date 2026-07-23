using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Infrastructure.Persistence;
using CarLog.Reminder.Infrastructure.Repositories;

namespace CarLog.Reminder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("AzureSqlConnection") ?? throw new InvalidOperationException("Connection string 'AzureSqlConnection' is not set.");

        services.AddDbContext<ReminderDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

                sqlOptions.MigrationsAssembly(typeof(ReminderDbContext).Assembly.FullName);

                sqlOptions.CommandTimeout(30);
            });

            if (environment.IsDevelopment() && configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IReminderRepository, ReminderRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks().AddDbContextCheck<ReminderDbContext>(name: "reminder-sql-db", tags: new[] { "database", "sql", "azure" });

        return services;
    }
}