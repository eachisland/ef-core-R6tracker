using R6tracker.Core.DTOs;

namespace R6tracker.Core.Interfaces;

public interface IRankService
{
    Task<IEnumerable<RankDto>> GetAllAsync();
    Task<RankDto> GetByIdAsync(string id);
}