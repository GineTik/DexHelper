using System.Collections;
using System.Globalization;
using System.Text.Json;
using Backend.Core.Interfaces.Token.TokensApi;
using Backend.Core.Interfaces.Token.TokensApi.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.Token.TokensApi.Types;
using Backend.Infrastructure.Implementation.Token.TokensApi.Types.PumpPortal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Backend.Infrastructure.Implementation.Token.TokensApi;

public class TokensApiClient : AbstractTokenApiClient, ITokensApiClient
{
    private readonly ILogger<TokensApiClient> _logger;
    private readonly PumpPortalFunOptions _pumpPortalFunOptions;
    
    public TokensApiClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<TokensApiClient> logger, IOptions<PumpPortalFunOptions> pumpPortalFunOptions) 
        : base(bitqueryOptions, logger, pumpPortalFunOptions)
    {
        _logger = logger;
        _pumpPortalFunOptions = pumpPortalFunOptions.Value;
    }
    
    public void SubscribeOnNewTokens(Func<NewToken, Task> callback)
    {
        SubscribeOnBitquery(
            async (response) =>
            {
                Console.WriteLine("test");
                foreach (var item in response.Solana.Instructions)
                {
                    var instruction = item.Instruction;
                    var transaction = item.Transaction;
                    var ipfsUrl = new List<dynamic>(instruction.Program.Arguments).First(o => ((string)o.Name).ToLower() == "uri").Value["string"];

                    try
                    {
                        var httpClient = new HttpClient();
                        var stringIpfsResponse = await httpClient.GetStringAsync(ipfsUrl.ToString());
                        using var doc = JsonDocument.Parse(stringIpfsResponse);
                        var root = doc.RootElement;
                        var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(root.GetProperty("data").GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        
                        var token = new NewToken
                        {
                            CreatedAtUtc = DateTime.UtcNow,
                            TokenAddress = instruction.Accounts[0],
                            PoolAddress = instruction.Accounts[2],
                            Wallet = instruction.Accounts[7],
                            TransactionSignature = transaction.Signature,
                            Name = ipfsResponse.Name,
                            Symbol = ipfsResponse.Symbol,
                            Description = ipfsResponse.Description,
                            ImageUrl = ipfsResponse.Image,
                            Website = ipfsResponse.Website,
                            Twitter = ipfsResponse.Twitter,
                            Telegram = ipfsResponse.Telegram,
                        };

                        await callback(token);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                    }
                }
            },
            @"
            subscription {
                Solana {
                    Instructions(
                        where: {Instruction: {Program: {Method: {is: ""create""}, Name: {is: ""pump""}}}}
                    ) {
                        Instruction {
                            Accounts {
                                Address
                                Token {
                                    Owner
                                }
                            }
                            Program {
                                Address
                                Arguments {
                                    Name
                                    Type
                                    Value {
                                        ... on Solana_ABI_Json_Value_Arg {
                                          json
                                        }
                                        ... on Solana_ABI_Float_Value_Arg {
                                          float
                                        }
                                        ... on Solana_ABI_Boolean_Value_Arg {
                                          bool
                                        }
                                        ... on Solana_ABI_Bytes_Value_Arg {
                                          hex
                                        }
                                        ... on Solana_ABI_BigInt_Value_Arg {
                                          bigInteger
                                        }
                                        ... on Solana_ABI_Address_Value_Arg {
                                          address
                                        }
                                        ... on Solana_ABI_String_Value_Arg {
                                          string
                                        }
                                        ... on Solana_ABI_Integer_Value_Arg {
                                          integer
                                        }
                                    }
                                }
                                Method
                                Name
                                AccountNames
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
        
        // SubscribeOnPumpPortal<NewTokenResponse>(
        //     async (newToken) =>
        //     {
        //         var httpClient = new HttpClient();
        //         var stringIpfsResponse = await httpClient.GetStringAsync(newToken.Uri);
        //         using var doc = JsonDocument.Parse(stringIpfsResponse);
        //         var root = doc.RootElement;
        //         var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(root.GetProperty("data").GetRawText(), new JsonSerializerOptions
        //         {
        //             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //         });
        //
        //         if (ipfsResponse == null)
        //             return;
        //         
        //         await callback(new NewToken
        //         {
        //             Name = newToken.Name,
        //             Symbol = newToken.Symbol,
        //             Description = ipfsResponse.Description,
        //             CreatedAtUtc = DateTime.UtcNow,
        //             ImageUrl = ipfsResponse.Image,
        //             TransactionSignature = newToken.Signature,
        //             TokenAddress = newToken.Mint,
        //             PoolAddress = newToken.BondingCurveKey,
        //             Website = ipfsResponse.Website,
        //             Twitter = ipfsResponse.Twitter,
        //             Telegram = ipfsResponse.Telegram,
        //             Wallet = newToken.TraderPublicKey
        //         });
        //     },
        //     new
        //     {
        //         method = "subscribeNewToken"
        //     }
        // );
    }
    public void SubscribeOnNewTransaction(Func<IEnumerable<NewTransaction>, Task> callback)
    {
        SubscribeOnBitquery(async (response) =>
            {
                var transactions = new List<dynamic>(response.Solana.DEXTrades).Select(o => new NewTransaction
                {
                    Type = o.Instruction.Program.Method,
                    BoughtTokenMintAddress = o.Trade.Buy.Currency.MintAddress,
                    SoldTokenMinAddress = o.Trade.Sell.Currency.MintAddress,
                    BoughtTokenAmount = o.Trade.Buy.Amount,
                    SoldTokenAmount = o.Trade.Sell.Amount,
                    BoughtTokenPriceUsd = o.Trade.Buy.PriceInUSD == 0 
                        ? o.Trade.Buy.AmountInUSD / decimal.Parse(o.Trade.Buy.Amount.ToString(), CultureInfo.InvariantCulture)
                        : o.Trade.Buy.PriceInUSD,
                    SoldTokenPriceUsd = o.Trade.Sell.PriceInUSD == 0 
                        ? o.Trade.Sell.AmountInUSD / decimal.Parse(o.Trade.Sell.Amount.ToString(), CultureInfo.InvariantCulture)
                        : o.Trade.Sell.PriceInUSD,
                    CreatedAtUtc = DateTime.UtcNow,
                    Signature = o.Transaction.Signature,
                    Wallet = o.Instruction.Program.Method == "buy" 
                        ? o.Trade.Buy.Account.Token.Owner
                        : o.Trade.Sell.Account.Token.Owner
                });
                await callback(transactions);
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
                            Name
                            Symbol
                            MintAddress
                            Decimals
                            Fungible
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