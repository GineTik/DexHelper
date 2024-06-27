using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Core.Interfaces.Bitquery;
using Backend.Core.Interfaces.Bitquery.Types;
using Backend.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Websocket.Client;

namespace Backend.Infrastructure.Implementation.Bitquery;

public class BitqueryClient : AbstractBitqueryClient, IBitqueryClient
{
    public BitqueryClient(IOptions<BitqueryOptions> bitqueryOptions, ILogger<BitqueryClient> logger) : base(bitqueryOptions, logger)
    {
    }
    
    public void SubscribeOnNewTokens(Action<NewTokensPayload> callback)
    {
        Subscribe(
            "new tokens",
            @"
                    subscription {
                      Solana {
                        Instructions(
                          where: {
                            Instruction: {
                                Program: {
                                    Address: {is: ""675kPX9MHTjS2zt1qfr1NYHuzeLXfQM9H24wFSUt1Mp8""}, 
                                    Method: {is: ""initializeUserWithNonce""}
                                }
                            }, 
                            Transaction: {Result: {Success: true}}
                         }
                        ) {
                          Transaction {
                            Signature
                          }
                          Block {
                            Time
                          }
                          Instruction {
                            Accounts {
                              Address
                            }
                            Program {
                              Address
                              Method
                              Arguments {
                                Name
                                Type
                                Value {
                                  ... on Solana_ABI_Integer_Value_Arg {
                                    integer
                                  }
                                  ... on Solana_ABI_String_Value_Arg {
                                    string
                                  }
                                  ... on Solana_ABI_Address_Value_Arg {
                                    address
                                  }
                                  ... on Solana_ABI_BigInt_Value_Arg {
                                    bigInteger
                                  }
                                  ... on Solana_ABI_Bytes_Value_Arg {
                                    hex
                                  }
                                  ... on Solana_ABI_Boolean_Value_Arg {
                                    bool
                                  }
                                  ... on Solana_ABI_Float_Value_Arg {
                                    float
                                  }
                                  ... on Solana_ABI_Json_Value_Arg {
                                    json
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
            ",
            callback
        );
    }

    public void SubscribeOnLastTokenTrade(string smartContract, Action<LastTradePayload> callback)
    {
        throw new NotImplementedException();
    }
}