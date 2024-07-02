using Backend.Core.Futures.TokenFiltration;
using Backend.Domain.Options;
using Backend.Infrastructure;
using Backend.SignalRHubs;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ConfigurationManager>(_ => builder.Configuration);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddInfrastructure();
builder.Services.Configure<BitqueryOptions>(builder.Configuration.GetSection(BitqueryOptions.Name));
builder.Services.Configure<PageOptions>(builder.Configuration.GetSection(PageOptions.Name));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(SearchNewTokensHandler).Assembly));
builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<TokensHub>("/ws/new-tokens");

SubscribeToNewTokens();
app.Run();

void SubscribeToNewTokens()
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var mediator = serviceProvider.GetRequiredService<IMediator>();
    mediator.Send(new SearchNewTokensRequest());
}