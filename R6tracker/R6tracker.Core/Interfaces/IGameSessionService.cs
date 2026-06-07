using R6tracker.Core.DTOs;

namespace R6tracker.Core.Interfaces;

public interface IGameSessionService
{
    Task<IEnumerable<GameSessionDto>> GetAllAsync();
    Task<IEnumerable<GameSessionDto>> GetByPlayerIdAsync(string playerId);
    Task<GameSessionDto?> GetByIdAsync(string id);
    Task<GameSessionDto> CreateAsync(CreateGameSessionDto dto);
    Task<bool> DeleteAsync(string id);
}