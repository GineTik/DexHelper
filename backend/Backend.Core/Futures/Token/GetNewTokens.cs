using Backend.Core.Futures.Token.Types;
using Backend.Core.Gateways;
using Backend.Domain.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Backend.Core.Futures.Token;

public class GetNewTokensResponse : TokenPaginationPage<TokenResponse>
{
    
}

public record GetNewTokensRequest(int Page) : IRequest<GetNewTokensResponse>
{
}

internal class GetNewTokensHandler : IRequestHandler<GetNewTokensRequest, GetNewTokensResponse>
{
    private readonly ITokenGateway _tokenGateway;
    private readonly PageOptions _pageOptions;
    
    public GetNewTokensHandler(ITokenGateway tokenGateway, IOptions<PageOptions> pageOptions)
    {
        _tokenGateway = tokenGateway;
        _pageOptions = pageOptions.Value;
    }

    public async Task<GetNewTokensResponse> Handle(GetNewTokensRequest request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;

        var totalSize = await _tokenGateway.GetTotalSize();
        var totalPages = totalSize % _pageOptions.TokenSize == 0
            ? totalSize / _pageOptions.TokenSize
            : (totalSize / _pageOptions.TokenSize) + 1;

        if (page > totalPages)
            page = totalPages;
            
        var offset = _pageOptions.TokenSize * (page - 1);
        var size = _pageOptions.TokenSize;
        var tokens = (await _tokenGateway.GetNewTokens(offset, size)).ToList();

        return new GetNewTokensResponse
        {
            Page = page,
            PageSize = _pageOptions.TokenSize,
            TotalSize = totalSize,
            TotalPages = totalPages,
            Items = tokens.Select(o => new TokenResponse
            {
                Name = o.Name,
                Symbol = o.Symbol,
                Image = o.ImageUrl,
                TokenAddress = o.TokenAddress,
                CreatedAtUtc = o.CreatedAtUtc
            })
        };
    }
}