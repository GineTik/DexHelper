using Backend;
using Backend.Core.Futures.TokenFiltration;
using Backend.Infrastructure;
using Backend.SignalRHubs;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ConfigurationManager>(_ => builder.Configuration);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddInfrastructure();
builder.Services.AddPresentation();

var app = builder.Build();
app.UseCors(policy => policy.WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
app.MapControllers();
app.MapHub<TokensHub>("/ws/new-tokens");

SubscribeNewTokens();
app.Run();

void SubscribeNewTokens()
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var mediator = serviceProvider.GetRequiredService<IMediator>();
    mediator.Send(new SearchNewTokensRequest());
}