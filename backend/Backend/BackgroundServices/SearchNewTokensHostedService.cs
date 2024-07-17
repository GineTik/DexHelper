using Backend.Core.Futures.TokenFiltration;
using Backend.Core.Futures.TokenFiltration.Types;
using Backend.SignalRHubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Backend.BackgroundServices;

public class SearchNewTokensHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public SearchNewTokensHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SearchNewTokensRequest(), stoppingToken);
    }
}