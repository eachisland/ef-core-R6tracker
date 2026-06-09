using R6tracker.Core.DTOs;

namespace R6tracker.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResultDto> LoginAsync(string email, string password);
    Task RegisterAsync(string email, string password, string displayName, string country);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}