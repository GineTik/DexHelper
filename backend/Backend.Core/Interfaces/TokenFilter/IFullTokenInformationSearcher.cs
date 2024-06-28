using Backend.Core.Interfaces.Bitquery.Types;
using Backend.Core.Interfaces.TokenFilter.Types;

namespace Backend.Core.Interfaces.TokenFilter;

public interface IFullTokenInformationSearcher
{
    FullTokenInformation Search(NewTokensPayload token);
}