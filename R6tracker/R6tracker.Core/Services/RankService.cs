using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Exceptions;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;

namespace R6tracker.Core.Services;

public class RankService : IRankService
{
    private readonly R6trackerDbContext context;
    private readonly ILogger<RankService> logger;

    public RankService(R6trackerDbContext context, ILogger<RankService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

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
        var rank = await context.Ranks
            .Where(r => r.Id == id)
            .Select(r => new RankDto
            {
                Id = r.Id,
                Name = r.Name,
                Tier = r.Tier
            })
            .FirstOrDefaultAsync();

        if (rank == null)
        {
            logger.LogWarning("Rank {Id} not found", id);
            throw new NotFoundException($"Rank with id {id} was not found.");
        }

        return rank;
    }
}