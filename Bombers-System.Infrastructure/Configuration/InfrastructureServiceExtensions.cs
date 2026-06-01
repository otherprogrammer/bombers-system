using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Adapters;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bombers_System.Infrastructure.Configuration;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database Connection
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        
        // Services Register
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        
        return services;
    }
}