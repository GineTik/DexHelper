using Backend.Core.Gateways;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Core.Futures.Token;

public class GetTokenInformationResponse
{
    public string Name { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public string Image { get; set; } = default!;
    public string TokenAddress { get; set; } = default!;
    public string Description { get; set; } = default!;
    public IEnumerable<Social> Links { get; set; } = default!;
}

public record GetTokenInformationRequest(string Address) : IRequest<GetTokenInformationResponse>
{
}

internal class GetTokenInformationHandler : IRequestHandler<GetTokenInformationRequest, GetTokenInformationResponse>
{
    private readonly ITokenGateway _tokenGateway;
    
    public GetTokenInformationHandler(ITokenGateway tokenGateway)
    {
        _tokenGateway = tokenGateway;
    }

    public async Task<GetTokenInformationResponse> Handle(GetTokenInformationRequest request, CancellationToken cancellationToken)
    {
        var token = await _tokenGateway.GetTokenInformation(request.Address);
        if (token == null)
            return new GetTokenInformationResponse();

        var links = new List<Social>
        {
            new() { Name = nameof(token.Website), Url = token.Website },
            new() { Name = nameof(token.Telegram), Url = token.Telegram },
            new() { Name = nameof(token.Twitter), Url = token.Twitter }
        };
        
        return new GetTokenInformationResponse
        {
            Name = token.Name,
            Symbol = token.Symbol,
            Image = token.ImageUrl,
            TokenAddress = token.TokenAddress,
            Description = token.Description,
            Links = links.Where(o => o.Url != null)
        };
    }
}