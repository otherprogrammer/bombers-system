using Bombers_System.Domain.Interfaces;
using Bombers_System.Infrastructure.Persistence;
using Bombers_System.Infrastructure.Repositories;
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
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
        });

        // Services Register
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;
    }
}
