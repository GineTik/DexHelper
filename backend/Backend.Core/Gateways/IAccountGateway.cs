namespace Backend.Core.Gateways;

public interface IAccountGateway
{
    Task AddIfNotExistsAsync(string wallet);
}