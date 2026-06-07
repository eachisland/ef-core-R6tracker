using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Core.Services;

public class GameSessionService(R6trackerDbContext context, ILogger<GameSessionService> logger) : IGameSessionService
{
    public async Task<IEnumerable<GameSessionDto>> GetAllAsync()
    {
        logger.LogInformation("Fetching all game sessions");

        return await context.GameSessions
            .Include(s => s.Player)
            .OrderByDescending(s => s.DatePlayed)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                Map = s.Map,
                PlayerId = s.PlayerId,
                PlayerName = s.Player == null ? string.Empty : s.Player.PlayerName ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<GameSessionDto>> GetByPlayerIdAsync(string playerId)
    {
        logger.LogInformation("Fetching sessions for player {PlayerId}", playerId);

        return await context.GameSessions
            .Include(s => s.Player)
            .Where(s => s.PlayerId == playerId)
            .OrderByDescending(s => s.DatePlayed)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                Map = s.Map,
                PlayerId = s.PlayerId,
                PlayerName = s.Player == null ? string.Empty : s.Player.PlayerName ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<GameSessionDto?> GetByIdAsync(string id)
    {
        return await context.GameSessions
            .Include(s => s.Player)
            .Where(s => s.Id == id)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                Map = s.Map,
                PlayerId = s.PlayerId,
                PlayerName = s.Player == null ? string.Empty : s.Player.PlayerName ?? string.Empty
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GameSessionDto> CreateAsync(CreateGameSessionDto dto)
    {
        var session = new GameSession
        {
            DatePlayed = dto.DatePlayed,
            Kills = dto.Kills,
            Deaths = dto.Deaths,
            Result = dto.Result,
            Map = dto.Map,
            PlayerId = dto.PlayerId
        };

        context.GameSessions.Add(session);
        await context.SaveChangesAsync();

        logger.LogInformation("Session {Id} created for player {PlayerId}", session.Id, session.PlayerId);

        return new GameSessionDto
        {
            Id = session.Id,
            DatePlayed = session.DatePlayed,
            Kills = session.Kills,
            Deaths = session.Deaths,
            Result = session.Result,
            Map = session.Map,
            PlayerId = session.PlayerId
        };
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var session = await context.GameSessions.FindAsync(id);

        if (session == null)
        {
            logger.LogWarning("Delete failed - session {Id} not found", id);
            return false;
        }

        context.GameSessions.Remove(session);
        await context.SaveChangesAsync();
        logger.LogInformation("Session {Id} deleted", id);
        return true;
    }
}