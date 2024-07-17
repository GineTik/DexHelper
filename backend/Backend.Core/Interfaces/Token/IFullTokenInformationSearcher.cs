using Backend.Core.Interfaces.Token.TokensApi.Types;
using Backend.Core.Interfaces.Token.Types;

namespace Backend.Core.Interfaces.Token;

public interface IFullTokenInformationSearcher
{
    FullTokenInformation Search(NewToken token);
}