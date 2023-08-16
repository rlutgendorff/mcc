using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mcc.Di.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterOpenGenericTypes(
        this IServiceCollection services, 
        Type openGenericType, 
        Assembly[] assemblies, 
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        var implementations = assemblies.SelectMany(x=>x.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterfaces().Any(interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == openGenericType))
            .ToList();

        foreach (var implementation in implementations)
        {
            var matchingInterface = implementation.GetInterfaces()
                .FirstOrDefault(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == openGenericType);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(matchingInterface, implementation);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(matchingInterface, implementation);
                    break;
                case ServiceLifetime.Transient:
                default:
                    services.AddTransient(matchingInterface, implementation);
                    break;
            }
        }
    }
}