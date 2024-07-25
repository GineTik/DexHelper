using Backend.Core.Futures.Token;
using Backend.Core.Futures.Token.Trackers;
using Backend.Core.Futures.Token.Trackers.TrackNewToken;
using Backend.Core.Futures.Token.Trackers.TrackNewTransactions;
using MediatR;

namespace Backend.BackgroundServices;

public class TrackCryptoHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public TrackCryptoHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new TrackNewTokenRequest(), cancellationToken);
        await mediator.Send(new TrackNewTransactionsRequest(), cancellationToken);

        while (cancellationToken.IsCancellationRequested == false)
        {
            await Task.Delay(1000, cancellationToken);
        }
    }
}