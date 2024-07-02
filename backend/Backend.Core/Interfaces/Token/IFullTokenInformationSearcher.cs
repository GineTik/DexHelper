using Backend.Core.Interfaces.TokensApi.Types;
using Backend.Core.Interfaces.Token.Types;

namespace Backend.Core.Interfaces.Token;

public interface IFullTokenInformationSearcher
{
    FullTokenInformation Search(NewToken token);
}