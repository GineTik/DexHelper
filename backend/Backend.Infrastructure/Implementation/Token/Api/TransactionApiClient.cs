using System.Globalization;
using Backend.Core.Interfaces.Token.Api;
using Backend.Core.Interfaces.Token.Api.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.Token.Api.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Implementation.Token.Api;

public class TransactionApiClient : AbstractTokenApiClient, ITransactionApiClient
{
    private readonly ILogger<TransactionApiClient> _logger;
    
    public TransactionApiClient(IOptions<BitqueryOptions> bitqueryOptions, IOptions<PumpPortalFunOptions> pumpPortalFunOptions, ILogger<TransactionApiClient> logger) : base(bitqueryOptions, logger, pumpPortalFunOptions)
    {
        _logger = logger;
    }
    
    public void SubscribeOnNewTransaction(Func<IEnumerable<TransactionInformation>, Task> callback)
    {
        SubscribeOnBitquery(async (response) =>
            {
                try
                {
                    var transactions = new List<dynamic>(response.Solana.DEXTrades).Select(o => new TransactionInformation
                    {
                        Type = o.Instruction.Program.Method,
                        BoughtToken = new NewTransactionTokenSide
                        {
                            Address = o.Trade.Buy.Currency.MintAddress,
                            Amount = o.Trade.Buy.Amount,
                            PriceUsd = o.Trade.Buy.PriceInUSD == 0
                                ? o.Trade.Buy.AmountInUSD / decimal.Parse(o.Trade.Buy.Amount.ToString(), CultureInfo.InvariantCulture)
                                : o.Trade.Buy.PriceInUSD,
                            Uri = o.Trade.Buy.Currency.Uri,
                        },
                        SoldToken = new NewTransactionTokenSide
                        {
                            Address = o.Trade.Sell.Currency.MintAddress,
                            Amount = o.Trade.Sell.Amount,
                            PriceUsd = o.Trade.Sell.PriceInUSD == 0
                                ? o.Trade.Sell.AmountInUSD / decimal.Parse(o.Trade.Sell.Amount.ToString(), CultureInfo.InvariantCulture)
                                : o.Trade.Sell.PriceInUSD,
                            Uri = o.Trade.Sell.Currency.Uri,
                        },
                        CreatedAtUtc = DateTime.UtcNow,
                        Signature = o.Transaction.Signature,
                        Wallet = o.Instruction.Program.Method == "buy"
                            ? o.Trade.Buy.Account.Token.Owner
                            : o.Trade.Sell.Account.Token.Owner
                    });
                    await callback(transactions);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            },
            @"
                subscription MyQuery {
                  Solana {
                    DEXTrades(
                      where: {Trade: {Dex: {ProtocolName: {is: ""pump""}}}, Transaction: {Result: {Success: true}}}
                    ) {
                      Instruction {
                        Program {
                          Method
                        }
                      }
                      Trade {
                        Dex {
                          ProtocolFamily
                          ProtocolName
                        }
                        Buy {
                          Amount
                          AmountInUSD
                          Account {
                            Address
                            Token {
                              Owner
                            }
                          }
                          Currency {
                            MintAddress
                            Uri
                          }
                          PriceInUSD
                        }
                        Sell {
                          Amount
                          AmountInUSD
                          Account {
                            Address
                            Token {
                              Owner
                            }
                          }
                          Currency {
                            MintAddress
                            Uri
                          }
                          PriceInUSD
                        }
                      }
                      Transaction {
                        Signature
                      }
                    }
                  }
                }
            "
        );
    }
}