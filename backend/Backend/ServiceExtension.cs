using System.Reflection;
using Backend.Core.Futures.Token.Trackers;
using Backend.Core.Futures.Token.Trackers.TrackNewToken;
using Backend.Domain.Options;

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
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(TrackNewTokenRequest).Assembly));
        services.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = true;
            hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(60);
        });
        services.AddCors();
        services.AddControllers();
        
        return services;
    }
}