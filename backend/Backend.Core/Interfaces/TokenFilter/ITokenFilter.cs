using Backend.Core.Interfaces.TokenFilter.Types;

namespace Backend.Core.Interfaces.TokenFilter;

public interface ITokenFilter
{
    Task<bool> Filter(FullTokenInformation information);
}