using Backend.Core.Interfaces.Bitquery.Types;

namespace Backend.Core.Interfaces.Bitquery;

public interface IBitqueryClient : IDisposable
{
    void SubscribeOnNewTokens(Func<NewToken, Task> callback);
}