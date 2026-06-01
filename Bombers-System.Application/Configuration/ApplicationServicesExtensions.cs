using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Bombers_System.Application.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        
        return services;
    }
}