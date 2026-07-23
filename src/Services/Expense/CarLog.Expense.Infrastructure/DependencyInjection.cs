using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarLog.Expense.Application.Common.Interfaces;
using CarLog.Expense.Infrastructure.Persistence;
using CarLog.Expense.Infrastructure.Repositories;

namespace CarLog.Expense.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("AzureSqlConnection") ?? throw new InvalidOperationException("Connection string 'AzureSqlConnection' is not set.");

        services.AddDbContext<ExpenseDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

                sqlOptions.MigrationsAssembly(typeof(ExpenseDbContext).Assembly.FullName);

                sqlOptions.CommandTimeout(30);
            });

            if (environment.IsDevelopment() && configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IExpenseRepository, ExpenseRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHealthChecks().AddDbContextCheck<ExpenseDbContext>(name: "expense-sql-db", tags: new[] { "database", "sql", "azure" });

        return services;
    }
}