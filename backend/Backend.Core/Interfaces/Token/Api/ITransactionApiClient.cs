using Backend.Core.Interfaces.Token.Api.Types;

namespace Backend.Core.Interfaces.Token.Api;

public interface ITransactionApiClient
{
    void SubscribeOnNewTransaction(Func<IEnumerable<TransactionInformation>, Task> callback);
}