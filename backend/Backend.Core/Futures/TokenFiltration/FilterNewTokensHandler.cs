using Backend.Core.Interfaces.Bitquery.Types;
using MediatR;

namespace Backend.Core.Futures.TokenFiltration;

public record FilterTokensCommand(NewTokensPayload Payload) : IRequest;

public class FilterNewTokensHandler : IRequestHandler<FilterTokensCommand>
{
    public Task Handle(FilterTokensCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}