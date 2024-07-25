using Backend.Core.Gateways;
using Backend.Core.Interfaces.Token.Api;
using Backend.Infrastructure.EF;
using Backend.Infrastructure.Gateways;
using Backend.Infrastructure.Implementation.Token.Api;
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
        services.AddSingleton<ITokenApiClient, TokenApiClient>();
        services.AddSingleton<ITransactionApiClient, TransactionApiClient>();
        services.AddScoped<ITokenGateway, TokenGateway>();
        services.AddScoped<ITransactionGateway, TransactionGateway>();
        services.AddScoped<IAccountGateway, AccountGateway>();
        
        return services;
    }
}