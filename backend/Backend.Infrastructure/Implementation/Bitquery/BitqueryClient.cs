using System.Text.Json;
using Backend.Core.Interfaces.Bitquery;
using Backend.Core.Interfaces.Bitquery.Types;
using Backend.Domain.Entities;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.Bitquery.Types;
using Backend.Infrastructure.Implementation.Bitquery.Types.PumpPortal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Websocket.Client;

namespace Backend.Infrastructure.Implementation.Bitquery;

public class BitqueryClient : AbstractBitqueryClient, IBitqueryClient
{
    private readonly ILogger<BitqueryClient> _logger;
    public BitqueryClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<BitqueryClient> logger) : base(bitqueryOptions, logger)
    {
        _logger = logger;
    }
    
    public void SubscribeOnNewTokens(Func<NewToken, Task> callback)
    {
        SubscribeOnPumpPortal<NewTokenResponse>(
            async (newToken) =>
            {
                var httpClient = new HttpClient();
                var stringIpfsResponse = await httpClient.GetStringAsync($"https://pumpportal.fun/api/data/token-info?ca={newToken.Mint}");
                using var doc = JsonDocument.Parse(stringIpfsResponse);
                var root = doc.RootElement;
                var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(root.GetProperty("data").GetRawText(), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (ipfsResponse == null)
                    return;
                
                await callback(new NewToken
                {
                    Name = ipfsResponse.Name,
                    Symbol = ipfsResponse.Symbol,
                    Description = ipfsResponse.Description,
                    CreatedAtUtc = DateTime.UtcNow,
                    ImageUrl = ipfsResponse.Image,
                    TransactionSignature = newToken.Signature,
                    TokenAddress = newToken.Mint,
                    PoolAddress = newToken.BondingCurveKey,
                    Website = ipfsResponse.Website,
                    Twitter = ipfsResponse.Twitter,
                    Telegram = ipfsResponse.Telegram,
                });
            },
            new
            {
                method = "subscribeNewToken"
            }
        );

        // Subscribe<NewTokensPayload>(
        //     async response =>
        //     {
        //         try
        //         {
        //             var token = response.Solana.Instructions.First();
        //             var ipfsResponse = await GetIpfsResponse(token.Instruction.Program.Arguments.First(o => o.Name == "uri").Value.String);
        //             await callback(new NewToken
        //             {
        //                 PoolAddress = token.Instruction.Accounts[4].Address,
        //                 TokenAddress = token.Instruction.Accounts[8].Address,
        //                 CreatedAt = token.Block.Time,
        //                 // Name = ipfsResponse.,
        //                 Symbol = token.Instruction.Program.Arguments.First(o => o.Name == "symbol").Value.String,
        //                 ImageUrl = ipfsResponse.Image,
        //                 TransactionSignature = token.Transaction.Signature
        //             });
        //         }
        //         catch (Exception e)
        //         {
        //             _logger.LogError(e.ToString());
        //         }
        //     },
        //     "new tokens",
        //     @"
        //       subscription {
        //           Solana {
        //               Instructions(
        //                   where: {Instruction: {Program: {Method: {is: ""create""}, Name: {is: ""pump""}}}}
        //               ) {
        //               Instruction {
        //                   Accounts {
        //                      Address
        //                   }
        //                   Program {
        //                     Address
        //                     Arguments {
        //                       Name
        //                       Type
        //                       Value {
        //                         ... on Solana_ABI_Json_Value_Arg {
        //                           json
        //                         }
        //                         ... on Solana_ABI_Float_Value_Arg {
        //                           float
        //                         }
        //                         ... on Solana_ABI_Boolean_Value_Arg {
        //                           bool
        //                         }
        //                         ... on Solana_ABI_Bytes_Value_Arg {
        //                           hex
        //                         }
        //                         ... on Solana_ABI_BigInt_Value_Arg {
        //                           bigInteger
        //                         }
        //                         ... on Solana_ABI_Address_Value_Arg {
        //                           address
        //                         }
        //                         ... on Solana_ABI_String_Value_Arg {
        //                           string
        //                         }
        //                         ... on Solana_ABI_Integer_Value_Arg {
        //                           integer
        //                         }
        //                       }
        //                     }
        //                     Method
        //                     Name
        //                   }
        //                 }
        //                 Transaction {
        //                   Signature
        //                 }
        //             }
        //         }
        //     }
        //     "
        // );
    }
    private static async Task<IpfsResponse> GetIpfsResponse(string ipfsUri)
    {
        var client = new HttpClient();
        var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(await client.GetStringAsync(ipfsUri), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return ipfsResponse!;
    }
}