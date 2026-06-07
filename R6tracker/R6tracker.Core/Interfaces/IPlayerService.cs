using R6tracker.Core.DTOs;

namespace R6tracker.Core.Interfaces;

public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>> GetAllAsync(string? search = null, string? platform = null);
    Task<PlayerDto?> GetByIdAsync(string id);
    Task<PlayerDto> CreateAsync(CreatePlayerDto dto, string userId);
    Task<bool> UpdateAsync(string id, CreatePlayerDto dto, string userId);
    Task<bool> DeleteAsync(string id, string userId, bool isAdmin);
}