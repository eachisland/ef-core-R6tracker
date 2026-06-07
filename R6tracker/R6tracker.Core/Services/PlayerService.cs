using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Core.Services;

public class PlayerService : IPlayerService
{
    private readonly R6trackerDbContext context;
    private readonly ILogger<PlayerService> logger;

    public PlayerService(R6trackerDbContext context, ILogger<PlayerService> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    public async Task<IEnumerable<PlayerDto>> GetAllAsync(string? search = null, string? platform = null)
    {
        logger.LogInformation("Fetching all players. Search={Search}, Platform={Platform}", search, platform);

        var query = context.R6Players
            .Include(p => p.Rank)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.PlayerName.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(platform))
        {
            query = query.Where(p => p.Platform == platform);
        }

        return await query
            .OrderByDescending(p => p.KillDeathRatio)
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                PlayerName = p.PlayerName,
                Platform = p.Platform,
                Level = p.Level,
                MatchesPlayed = p.MatchesPlayed,
                Kills = p.Kills,
                Deaths = p.Deaths,
                KillDeathRatio = p.KillDeathRatio,
                Country = p.Country,
                RankName = p.Rank != null ? p.Rank.Name : "Unranked",
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PlayerDto?> GetByIdAsync(string id)
    {
        logger.LogInformation("Fetching player {Id}", id);

        return await context.R6Players
            .Include(p => p.Rank)
            .Where(p => p.Id == id)
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                PlayerName = p.PlayerName,
                Platform = p.Platform,
                Level = p.Level,
                MatchesPlayed = p.MatchesPlayed,
                Kills = p.Kills,
                Deaths = p.Deaths,
                KillDeathRatio = p.KillDeathRatio,
                Country = p.Country,
                RankName = p.Rank != null ? p.Rank.Name : "Unranked",
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PlayerDto> CreateAsync(CreatePlayerDto dto, string userId)
    {
        bool exists = await context.R6Players
            .AnyAsync(p => p.PlayerName == dto.PlayerName && p.Platform == dto.Platform);

        if (exists)
        {
            logger.LogWarning("Player {Name} on {Platform} already exists", dto.PlayerName, dto.Platform);
            throw new InvalidOperationException("Player already exists on that platform.");
        }

        double kd = dto.Deaths == 0 ? dto.Kills : (double)dto.Kills / dto.Deaths;

        var player = new R6Player
        {
            PlayerName = dto.PlayerName,
            Platform = dto.Platform,
            Level = dto.Level,
            MatchesPlayed = dto.MatchesPlayed,
            Kills = dto.Kills,
            Deaths = dto.Deaths,
            KillDeathRatio = kd,
            Country = dto.Country,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        context.R6Players.Add(player);
        await context.SaveChangesAsync();

        logger.LogInformation("Player {Name} created with Id {Id}", player.PlayerName, player.Id);

        return new PlayerDto
        {
            Id = player.Id,
            PlayerName = player.PlayerName,
            Platform = player.Platform,
            Level = player.Level,
            MatchesPlayed = player.MatchesPlayed,
            Kills = player.Kills,
            Deaths = player.Deaths,
            KillDeathRatio = player.KillDeathRatio,
            Country = player.Country,
            RankName = "Unranked",
            CreatedAt = player.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(string id, CreatePlayerDto dto, string userId)
    {
        var player = await context.R6Players.FindAsync(id);

        if (player == null)
        {
            logger.LogWarning("Update failed - player {Id} not found", id);
            return false;
        }

        if (player.UserId != userId)
        {
            logger.LogWarning("Update forbidden - user {UserId} does not own player {Id}", userId, id);
            return false;
        }

        player.PlayerName = dto.PlayerName;
        player.Platform = dto.Platform;
        player.Level = dto.Level;
        player.MatchesPlayed = dto.MatchesPlayed;
        player.Kills = dto.Kills;
        player.Deaths = dto.Deaths;
        player.KillDeathRatio = dto.Deaths == 0 ? dto.Kills : (double)dto.Kills / dto.Deaths;
        player.Country = dto.Country;

        await context.SaveChangesAsync();
        logger.LogInformation("Player {Id} updated", id);
        return true;
    }

    public async Task<bool> DeleteAsync(string id, string userId, bool isAdmin)
    {
        var player = await context.R6Players.FindAsync(id);

        if (player == null)
        {
            logger.LogWarning("Delete failed - player {Id} not found", id);
            return false;
        }

        if (!isAdmin && player.UserId != userId)
        {
            logger.LogWarning("Delete forbidden - user {UserId} does not own player {Id}", userId, id);
            return false;
        }

        context.R6Players.Remove(player);
        await context.SaveChangesAsync();
        logger.LogInformation("Player {Id} deleted", id);
        return true;
    }
}