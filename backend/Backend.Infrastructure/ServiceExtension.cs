using Backend.Core.Gateways;
using Backend.Core.Interfaces.Bitquery;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Gateways;
using Backend.Infrastructure.Implementation.Bitquery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class ServiceExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<ConfigurationManager>();

        services.AddDbContext<DataContext>(o => o.UseSqlServer(configuration.GetConnectionString("MSSQL")));
        services.AddSingleton<IBitqueryClient, BitqueryClient>();
        services.AddScoped<ITokenGateway, TokenGateway>();
        
        return services;
    }
}