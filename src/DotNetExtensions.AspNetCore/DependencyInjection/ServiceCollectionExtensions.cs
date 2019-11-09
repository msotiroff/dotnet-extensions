using DotNetExtensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DotNetExtensions.AspNetCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConvetionallyNamedServices(
            this IServiceCollection services, 
            Assembly assembly = default, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            return services
                .AddConvetionallyNamedServices(
                    (serviceCollection, serviceType, implementationType) => 
                        serviceCollection.Add(new ServiceDescriptor(
                            serviceType, implementationType, serviceLifetime)), 
                    assembly);
        }
    }
}
