using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Exceptions;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Core.Services;

public class MapService : IMapService
{
    private readonly R6trackerDbContext context;
    private readonly ILogger<MapService> logger;

    public MapService(R6trackerDbContext context, ILogger<MapService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<MapDto>> GetAllAsync()
    {
        logger.LogInformation("Fetching all maps");

        return await context.Maps
            .OrderBy(m => m.Name)
            .Select(m => new MapDto
            {
                Id = m.Id,
                Name = m.Name,
                Location = m.Location,
                Type = m.Type,
                IsActive = m.IsActive
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<MapDto>> GetActiveAsync()
    {
        logger.LogInformation("Fetching active maps");

        return await context.Maps
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .Select(m => new MapDto
            {
                Id = m.Id,
                Name = m.Name,
                Location = m.Location,
                Type = m.Type,
                IsActive = m.IsActive
            })
            .ToListAsync();
    }

    public async Task<MapDto> GetByIdAsync(int id)
    {
        var map = await context.Maps
            .Where(m => m.Id == id)
            .Select(m => new MapDto
            {
                Id = m.Id,
                Name = m.Name,
                Location = m.Location,
                Type = m.Type,
                IsActive = m.IsActive
            })
            .FirstOrDefaultAsync();

        if (map == null)
        {
            logger.LogWarning("Map {Id} not found", id);
            throw new NotFoundException($"Map with id {id} was not found.");
        }

        return map;
    }

    public async Task<MapDto> CreateAsync(CreateMapDto dto)
    {
        var map = new R6Map
        {
            Name = dto.Name,
            Location = dto.Location,
            Type = dto.Type,
            IsActive = dto.IsActive
        };

        context.Maps.Add(map);
        await context.SaveChangesAsync();
        logger.LogInformation("Map {Name} created", map.Name);

        return new MapDto
        {
            Id = map.Id,
            Name = map.Name,
            Location = map.Location,
            Type = map.Type,
            IsActive = map.IsActive
        };
    }

    public async Task<bool> UpdateAsync(int id, CreateMapDto dto)
    {
        var map = await context.Maps.FindAsync(id);

        if (map == null)
        {
            logger.LogWarning("Update failed - map {Id} not found", id);
            throw new NotFoundException($"Map with id {id} was not found.");
        }

        map.Name = dto.Name;
        map.Location = dto.Location;
        map.Type = dto.Type;
        map.IsActive = dto.IsActive;

        await context.SaveChangesAsync();
        logger.LogInformation("Map {Id} updated", id);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var map = await context.Maps.FindAsync(id);

        if (map == null)
        {
            logger.LogWarning("Delete failed - map {Id} not found", id);
            throw new NotFoundException($"Map with id {id} was not found.");
        }

        context.Maps.Remove(map);
        await context.SaveChangesAsync();
        logger.LogInformation("Map {Id} deleted", id);
        return true;
    }
}