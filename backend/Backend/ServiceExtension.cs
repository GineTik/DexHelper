using Backend.BackgroundServices;
using Backend.Core.Futures.Token;
using Backend.Domain.Options;
using Backend.NotificationHandlers;

namespace Backend;

public static class ServiceExtension
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<ConfigurationManager>();
        
        // services.AddHostedService<SearchNewTokensHostedService>();
        services.Configure<BitqueryOptions>(configuration.GetSection(BitqueryOptions.Name));
        services.Configure<PumpPortalFunOptions>(configuration.GetSection(PumpPortalFunOptions.Name));
        services.Configure<PageOptions>(configuration.GetSection(PageOptions.Name));
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(NewTokenNotificationHandler).Assembly, typeof(SearchNewTokensHandler).Assembly));
        services.AddSignalR();
        services.AddCors();
        services.AddControllers();
        
        return services;
    }
}