using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using R6tracker.Core.DTOs;
using R6tracker.Core.Services;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Tests;

[TestFixture]
public class GameSessionServiceTests
{
    private R6trackerDbContext context;
    private Mock<ILogger<GameSessionService>> loggerMock;
    private GameSessionService service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<R6trackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new R6trackerDbContext(options);
        loggerMock = new Mock<ILogger<GameSessionService>>();
        service = new GameSessionService(context, loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllSessions()
    {
        var player = new R6Player { Id = "p1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" };
        context.R6Players.Add(player);

        var map = new R6Map { Name = "Border", Location = "Mexico", Type = "Bomb", IsActive = true };
        context.Maps.Add(map);
        await context.SaveChangesAsync();

        context.GameSessions.AddRange(
            new GameSession { Id = "s1", PlayerId = "p1", MapId = map.Id, Result = "Win", DatePlayed = DateTime.UtcNow },
            new GameSession { Id = "s2", PlayerId = "p1", MapId = map.Id, Result = "Loss", DatePlayed = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetByPlayerIdAsync_ReturnsOnlyPlayerSessions()
    {
        var player1 = new R6Player { Id = "p1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" };
        var player2 = new R6Player { Id = "p2", PlayerName = "NovaSix", Platform = "PS5", Country = "US", UserId = "user2" };
        context.R6Players.AddRange(player1, player2);

        var map = new R6Map { Name = "Border", Location = "Mexico", Type = "Bomb", IsActive = true };
        context.Maps.Add(map);
        await context.SaveChangesAsync();

        context.GameSessions.AddRange(
            new GameSession { Id = "s1", PlayerId = "p1", MapId = map.Id, Result = "Win", DatePlayed = DateTime.UtcNow },
            new GameSession { Id = "s2", PlayerId = "p2", MapId = map.Id, Result = "Loss", DatePlayed = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = await service.GetByPlayerIdAsync("p1");

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().PlayerId, Is.EqualTo("p1"));
    }

    [Test]
    public async Task CreateAsync_ValidSession_AddsToDatabase()
    {
        var player = new R6Player { Id = "p1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" };
        context.R6Players.Add(player);

        var map = new R6Map { Name = "Border", Location = "Mexico", Type = "Bomb", IsActive = true };
        context.Maps.Add(map);
        await context.SaveChangesAsync();

        var dto = new CreateGameSessionDto
        {
            PlayerId = "p1",
            MapId = map.Id,
            Result = "Win",
            DatePlayed = DateTime.UtcNow,
            Kills = 5,
            Deaths = 2
        };

        var result = await service.CreateAsync(dto);

        Assert.That(result.Result, Is.EqualTo("Win"));
        Assert.That(context.GameSessions.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteAsync_WhenSessionExists_DeletesSession()
    {
        var player = new R6Player { Id = "p1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" };
        context.R6Players.Add(player);

        var map = new R6Map { Name = "Border", Location = "Mexico", Type = "Bomb", IsActive = true };
        context.Maps.Add(map);
        await context.SaveChangesAsync();

        context.GameSessions.Add(
            new GameSession { Id = "s1", PlayerId = "p1", MapId = map.Id, Result = "Win", DatePlayed = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var result = await service.DeleteAsync("s1");

        Assert.That(result, Is.True);
        Assert.That(context.GameSessions.Count(), Is.EqualTo(0));
    }

    [Test]
    public void DeleteAsync_WhenSessionNotFound_ThrowsNotFoundException()
    {
        Assert.ThrowsAsync<R6tracker.Core.Exceptions.NotFoundException>(async () =>
            await service.DeleteAsync("notexist"));
    }
}