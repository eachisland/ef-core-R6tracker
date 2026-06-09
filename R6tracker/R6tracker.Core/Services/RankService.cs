using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;

namespace R6tracker.Core.Services;

public class RankService(R6trackerDbContext context, ILogger<RankService> logger) : IRankService
{
    public async Task<IEnumerable<RankDto>> GetAllAsync()
    {
        logger.LogInformation("Fetching all ranks");

        return await context.Ranks
            .OrderBy(r => r.Tier)
            .Select(r => new RankDto
            {
                Id = r.Id,
                Name = r.Name,
                Tier = r.Tier
            })
            .ToListAsync();
    }

    public async Task<RankDto> GetByIdAsync(string id)
    {
        return await context.Ranks
            .Where(r => r.Id == id)
            .Select(r => new RankDto
            {
                Id = r.Id,
                Name = r.Name,
                Tier = r.Tier
            })
            .FirstOrDefaultAsync();
    }
}