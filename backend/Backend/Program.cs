using Backend.Core.Futures.TokenFiltration;
using Backend.Domain.Options;
using Backend.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddInfrastructure();
builder.Services.Configure<BitqueryOptions>(builder.Configuration.GetSection(BitqueryOptions.Name));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(SubscribeBitqueryApi).Assembly));

var app = builder.Build();
app.UseWebSockets();

SubscribeToNewTokens();
app.Run();

void SubscribeToNewTokens()
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var mediator = serviceProvider.GetRequiredService<IMediator>();
    mediator.Send(new SubscribeBitqueryApiCommand());
}