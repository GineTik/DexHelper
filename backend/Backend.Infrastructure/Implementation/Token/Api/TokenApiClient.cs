using System.Collections;
using System.Text.Json;
using System.Xml;
using Backend.Core.Interfaces.Token.Api;
using Backend.Core.Interfaces.Token.Api.Types;
using Backend.Domain.Options;
using Backend.Infrastructure.Implementation.Token.Api.Base;
using Backend.Infrastructure.Implementation.Token.Api.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Implementation.Token.Api;

public class TokenApiClient : AbstractTokenApiClient, ITokenApiClient
{
    private readonly ILogger<TokenApiClient> _logger;
    private readonly PumpPortalFunOptions _pumpPortalFunOptions;
    
    private DateTime _lastSentIpfsRequest;
    private object _locker = new object();
    
    public TokenApiClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<TokenApiClient> logger, IOptions<PumpPortalFunOptions> pumpPortalFunOptions) 
        : base(bitqueryOptions, logger, pumpPortalFunOptions)
    {
        _logger = logger;
        _pumpPortalFunOptions = pumpPortalFunOptions.Value;
        _lastSentIpfsRequest = DateTime.Now;
    }
    
    public void SubscribeOnNewTokens(Func<TokenInformation, Task> callback)
    {
        SubscribeOnBitquery(
            async (response) =>
            {
                try
                {
                    foreach (var item in response.Solana.Instructions)
                    {
                        var token = await ParseTokenResponse(item);
                        await callback(token);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("[SubscribeOnNewTokens]: " + e.ToString());
                }
            },
            @"
            subscription {
                Solana {
                    Instructions(
                        where: {Instruction: {Program: {Method: {is: ""create""}, Name: {is: ""pump""}}}}
                    ) {
                        Block {
                            Time
                        }
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
    }
    
    public async Task<TokenInformation?> GetToken(string address)
    {
        var response = await SendQueryToBitquery(@"
        query {
            Solana {
                Instructions(
                    where: {Instruction: {Accounts: {includes: {Address: {is: """ + address + @"""}}}, Program: {Method: {is: ""create""}}}}
                ) {
                    Block {
                        Time
                    }
                    Transaction {
                        Signature
                    }
                    Instruction {
                        Program {
                              Method
                              AccountNames
                              Address
                              Arguments {
                                    Name
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
                            }
                            Accounts {
                              Address
                              Token {
                                Owner
                              }
                            }
                        }
                    }
                }
            }
        ");

        var introductionsLength = response.Solana.Instructions.Count;

        return introductionsLength == 0
            ? null
            : await ParseTokenResponse(response.Solana.Instructions[0]);
    }
    
    public async Task<IEnumerable<TokenInformation>> GetAllTokens(IEnumerable<string> exceptAddresses)
    {
        var response = await SendQueryToBitquery(@"
            query {
                Solana {
                    Instructions(
                        where: {Instruction: {Program: {Method: {is: ""create""}, Name: {is: ""pump""}}}}
                    ) {
                        Block {
                            Time
                        }
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
        ");
        
        try
        {
            var tokens = new List<TokenInformation>();
            var exceptAddressesList = exceptAddresses.ToList();
            var count = 0;

            foreach (var item in response.Solana.Instructions)
            {
                var tokenAddress = item.Instruction.Accounts[0].Address.ToString();
                if (exceptAddressesList.Contains(tokenAddress))
                    continue;
                
                var token = await ParseTokenResponse(item);
                tokens.Add(token);

                _logger.LogInformation((++count).ToString());
            }
            
            return tokens;
        }
        catch (Exception e)
        {
            _logger.LogError("[GetAllToken]: " + e.ToString());
            throw;
        }
    }

    private async Task<TokenInformation> ParseTokenResponse(dynamic instructionItem)
    {
        var instruction = instructionItem.Instruction;
        var transaction = instructionItem.Transaction;
        var ipfsUri = new List<dynamic>(instruction.Program.Arguments).First(o => ((string)o.Name).ToLower() == "uri").Value["string"];

        try
        {
            var ipfsResponse = await GetMoreTokenInformation(ipfsUri);

            return new TokenInformation
            {
                CreatedAtUtc = DateTime.UtcNow,
                TokenAddress = instruction.Accounts[0].Address,
                BoundingCurveAddress = instruction.Accounts[2].Address,
                AuthorWallet = instruction.Accounts[7].Address,
                CreationSignature = transaction.Signature,
                Name = ipfsResponse.Name,
                Symbol = ipfsResponse.Symbol,
                Description = ipfsResponse.Description,
                ImageUrl = ipfsResponse.Image,
                Website = ipfsResponse.Website,
                Twitter = ipfsResponse.Twitter,
                Telegram = ipfsResponse.Telegram,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
    }
    
    private async Task<dynamic> GetMoreTokenInformation(dynamic ipfsUrl)
    {
        var httpClient = new HttpClient();
        var stringIpfsResponse = await httpClient.GetStringAsync(ipfsUrl.ToString());
        using JsonDocument doc = JsonDocument.Parse(stringIpfsResponse);
        var root = doc.RootElement;
        var ipfsResponse = JsonSerializer.Deserialize<IpfsResponse>(root.GetRawText(), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })!;
        return ipfsResponse;
    }
}