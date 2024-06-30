using Backend.Core.Futures.TokenFiltration.Types;
using Backend.Core.Gateways;
using Backend.Domain.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Backend.Core.Futures.TokenFiltration;

public class GetNewTokensResponse
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
}

public class GetNewTokensRequest : IRequest<TokenPaginationPage<GetNewTokensResponse>>
{
    public int Page { get; set; }
}

public class GetNewTokensHandler : IRequestHandler<GetNewTokensRequest, TokenPaginationPage<GetNewTokensResponse>>
{
    private readonly ITokenGateway _tokenGateway;
    private readonly PageOptions _pageOptions;
    
    public GetNewTokensHandler(ITokenGateway tokenGateway, IOptions<PageOptions> pageOptions)
    {
        _tokenGateway = tokenGateway;
        _pageOptions = pageOptions.Value;
    }

    public async Task<TokenPaginationPage<GetNewTokensResponse>> Handle(GetNewTokensRequest request, CancellationToken cancellationToken)
    {
        if (request.Page < 1)
            request.Page = 1;

        var totalSize = await _tokenGateway.GetTotalSize();
        var totalPages = totalSize % _pageOptions.TokenSize == 0
            ? totalSize / _pageOptions.TokenSize
            : (totalSize / _pageOptions.TokenSize) + 1;

        if (request.Page > totalPages)
            request.Page = totalPages;
            
        var offset = _pageOptions.TokenSize * (request.Page - 1);
        var size = _pageOptions.TokenSize;
        var tokens = (await _tokenGateway.GetTokenList(offset, size)).ToList();

        return new TokenPaginationPage<GetNewTokensResponse>
        {
            Page = request.Page,
            PageSize = _pageOptions.TokenSize,
            TotalSize = totalSize,
            TotalPages = totalPages,
            Items = tokens.Select(o => new GetNewTokensResponse
            {
                Name = o.Name,
                Symbol = o.Symbol,
                ImageUrl = o.ImageUrl,
                TokenAddress = o.TokenAddress
            })
        };
    }
}