using R6tracker.Core.DTOs;

namespace R6tracker.Core.Interfaces;

public interface IMapService
{
    Task<IEnumerable<MapDto>> GetAllAsync();
    Task<IEnumerable<MapDto>> GetActiveAsync();
    Task<MapDto> GetByIdAsync(int id);
    Task<MapDto> CreateAsync(CreateMapDto dto);
    Task<bool> UpdateAsync(int id, CreateMapDto dto);
    Task<bool> DeleteAsync(int id);
}