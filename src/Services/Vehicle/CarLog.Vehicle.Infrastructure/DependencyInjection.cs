using CarLog.Vehicle.Application.Interfaces.Repositories;
using CarLog.Vehicle.Infrastructure.Persistence;
using CarLog.Vehicle.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarLog.Vehicle.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddDbContext<VehicleDbContext>(optionns => 
        {
            optionns.UseSqlServer(configuration.GetConnectionString(""), b => b.MigrationsAssembly(typeof(VehicleDbContext).Assembly.FullName));
        });

        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;
    }
}
