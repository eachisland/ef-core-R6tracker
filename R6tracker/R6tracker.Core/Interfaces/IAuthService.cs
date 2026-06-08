namespace R6tracker.Core.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task RegisterAsync(string email, string password, string displayName, string country);
}