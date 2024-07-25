using Backend.Core.Futures.Token.Trackers.TrackNewToken;
using Backend.Core.Gateways;
using Backend.Core.Interfaces.Token.Api;
using Backend.Core.Interfaces.Token.Api.Types;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Futures.Token.Trackers.TrackNewTransactions;


public record TrackNewTransactionsRequest() : IRequest;

public class NewTransactionNotification : Transaction, INotification {}

public enum AddTokenIfNotExistsStatus
{
    Added, 
    NotAdded
}

internal class TrackNewTransactionsHandler : IRequestHandler<TrackNewTransactionsRequest>
{
    private readonly ILogger<TrackNewTokenHandler> _logger;
    private readonly ITransactionApiClient _transactionApiClient;
    private readonly ITokenApiClient _tokenApiClient;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    public TrackNewTransactionsHandler(ILogger<TrackNewTokenHandler> logger, ITransactionApiClient transactionApiClient, IMediator mediator, IServiceProvider serviceProvider, ITokenApiClient tokenApiClient)
    {
        _logger = logger;
        _transactionApiClient = transactionApiClient;
        _mediator = mediator;
        _serviceProvider = serviceProvider;
        _tokenApiClient = tokenApiClient;
    }

    public Task Handle(TrackNewTransactionsRequest newTokenRequest, CancellationToken cancellationToken)
    {
        try
        {
            _transactionApiClient.SubscribeOnNewTransaction(async transactions =>
            {
                var convertedTransactions = await ConvertAndNotify(transactions, cancellationToken);
                await AddTransactionsToDb(cancellationToken, convertedTransactions);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR: " + ex.ToString());
        }
        
        return Task.CompletedTask;
    }
    
    private async Task<IEnumerable<Transaction>> ConvertAndNotify(IEnumerable<TransactionInformation> transactions, CancellationToken cancellationToken)
    {
        var convertedTransactions = new List<Transaction>();

        foreach (var transaction in transactions)
        {
            var convertedTransaction = Convert(transaction);
            await _mediator.Publish(convertedTransaction, cancellationToken);
            convertedTransactions.Add(convertedTransaction);
        }

        return convertedTransactions;
    }

    private async Task AddTransactionsToDb(CancellationToken cancellationToken, IEnumerable<Transaction> convertedTransactions)
    {
        using var scope = _serviceProvider.CreateScope();
        var tokenGateway = scope.ServiceProvider.GetRequiredService<ITokenGateway>();
        var transactionGateway = scope.ServiceProvider.GetRequiredService<ITransactionGateway>();
        var accountGateway = scope.ServiceProvider.GetRequiredService<IAccountGateway>();
        
        foreach (var transaction in convertedTransactions)
        {
            await accountGateway.AddIfNotExistsAsync(transaction.AccountWallet);

            if (await tokenGateway.IsExists(transaction.CryptoTokenAddress) == false)
                return;
            
            await transactionGateway.AddInNotExistsAsync(transaction);
            await Task.Delay(1000, cancellationToken);
        }
    }
    
    private static NewTransactionNotification Convert(TransactionInformation transaction)
    {
        var type = ParseStringTypeToEnum(transaction);
        var token = TakeTokenSideOfTradingToken(type, transaction);
        
        return new NewTransactionNotification
        {
            Signature = transaction.Signature,
            Type = type,
            BoughtTokenAmount = transaction.BoughtToken.Amount,
            SoldTokenAmount = transaction.SoldToken.Amount,
            BoughtTokenPriceUsd = transaction.BoughtToken.PriceUsd,
            SoldTokenPriceUsd = transaction.SoldToken.PriceUsd,
            CryptoTokenAddress = token.Address,
            AccountWallet = transaction.Wallet,
            CreatedAtUtc = transaction.CreatedAtUtc
        };  
    }
    
    private static NewTransactionTokenSide TakeTokenSideOfTradingToken(TransactionType type, TransactionInformation transaction)
    {
        return type switch
        {
            TransactionType.Buy => transaction.BoughtToken,
            TransactionType.Sell => transaction.SoldToken,
            _ => throw new NotImplementedException()
        };
    }
    
    private static TransactionType ParseStringTypeToEnum(TransactionInformation transaction)
    {
        return transaction.Type switch
        {
            "buy" => TransactionType.Buy,
            "sell" => TransactionType.Sell,
            _ => throw new NotImplementedException()
        };
    }
    //
    // private async Task<AddTokenIfNotExistsStatus> AddTokenIfNotExists(string address)
    // {
    //     using var scope = _serviceProvider.CreateScope();
    //     var tokenGateway = scope.ServiceProvider.GetRequiredService<ITokenGateway>();
    //
    //     if (await tokenGateway.IsExists(address))
    //         return AddTokenIfNotExistsStatus.Added;
    //
    //     var token = await _tokenApiClient.GetToken(address);
    //     if (token == null)
    //         return AddTokenIfNotExistsStatus.NotAdded;
    //
    //     await tokenGateway.AddAsync(new CryptoToken
    //     {
    //         Name = token.Name,
    //         Symbol = token.Symbol,
    //         Description = token.Description,
    //         ImageUrl = token.ImageUrl,
    //         CreationSignature = token.CreationSignature,
    //         CreatedAtUtc = token.CreatedAtUtc,
    //         Address = token.TokenAddress,
    //         BoundingCurveAddress = token.BoundingCurveAddress,
    //         UpdatedAtUtc = token.CreatedAtUtc,
    //         Website = token.Website,
    //         Twitter = token.Twitter,
    //         Telegram = token.Telegram,
    //     });
    //     
    //     return AddTokenIfNotExistsStatus.Added;
    // }
}