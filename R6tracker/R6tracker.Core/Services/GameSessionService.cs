using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using R6tracker.Core.DTOs;
using R6tracker.Core.Exceptions;
using R6tracker.Core.Interfaces;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Core.Services;

public class GameSessionService : IGameSessionService
{
    private readonly R6trackerDbContext context;
    private readonly ILogger<GameSessionService> logger;

    public GameSessionService(R6trackerDbContext context, ILogger<GameSessionService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<GameSessionDto>> GetAllAsync()
    {
        logger.LogInformation("Fetching all game sessions");

        return await context.GameSessions
            .Include(s => s.Player)
            .Include(s => s.Map)
            .OrderByDescending(s => s.DatePlayed)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                MapName = s.Map.Name,
                MapId = s.MapId,
                PlayerId = s.PlayerId,
                PlayerName = s.Player.PlayerName
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<GameSessionDto>> GetByPlayerIdAsync(string playerId)
    {
        logger.LogInformation("Fetching sessions for player {PlayerId}", playerId);

        return await context.GameSessions
            .Include(s => s.Player)
            .Include(s => s.Map)
            .Where(s => s.PlayerId == playerId)
            .OrderByDescending(s => s.DatePlayed)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                MapName = s.Map.Name,
                MapId = s.MapId,
                PlayerId = s.PlayerId,
                PlayerName = s.Player.PlayerName
            })
            .ToListAsync();
    }

    public async Task<GameSessionDto> GetByIdAsync(string id)
    {
        var session = await context.GameSessions
            .Include(s => s.Player)
            .Include(s => s.Map)
            .Where(s => s.Id == id)
            .Select(s => new GameSessionDto
            {
                Id = s.Id,
                DatePlayed = s.DatePlayed,
                Kills = s.Kills,
                Deaths = s.Deaths,
                Result = s.Result,
                MapName = s.Map.Name,
                MapId = s.MapId,
                PlayerId = s.PlayerId,
                PlayerName = s.Player.PlayerName
            })
            .FirstOrDefaultAsync();

        if (session == null)
        {
            logger.LogWarning("Session {Id} not found", id);
            throw new NotFoundException($"Session with id {id} was not found.");
        }

        return session;
    }

    public async Task<GameSessionDto> CreateAsync(CreateGameSessionDto dto)
    {
        var session = new GameSession
        {
            DatePlayed = dto.DatePlayed,
            Kills = dto.Kills,
            Deaths = dto.Deaths,
            Result = dto.Result,
            MapId = dto.MapId,
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
            MapId = session.MapId,
            PlayerId = session.PlayerId
        };
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var session = await context.GameSessions.FindAsync(id);

        if (session == null)
        {
            logger.LogWarning("Delete failed - session {Id} not found", id);
            throw new NotFoundException($"Session with id {id} was not found.");
        }

        context.GameSessions.Remove(session);
        await context.SaveChangesAsync();
        logger.LogInformation("Session {Id} deleted", id);
        return true;
    }
}