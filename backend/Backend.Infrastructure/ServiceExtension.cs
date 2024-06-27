using Backend.Core.Interfaces.Bitquery;
using Backend.Infrastructure.Implementation.Bitquery;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class ServiceExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IBitqueryClient, BitqueryClient>();
        return services;
    }
}