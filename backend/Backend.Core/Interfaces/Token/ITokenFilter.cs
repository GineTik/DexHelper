using Backend.Core.Interfaces.Token.Types;

namespace Backend.Core.Interfaces.Token;

public interface ITokenFilter
{
    Task<bool> Filter(FullTokenInformation information);
}