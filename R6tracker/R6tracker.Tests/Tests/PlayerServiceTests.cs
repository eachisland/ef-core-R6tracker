using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using R6tracker.Core.Services;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;
using R6tracker.Core.DTOs;

namespace R6tracker.Tests;

[TestFixture]
public class PlayerServiceTests
{
    private R6trackerDbContext context;
    private Mock<ILogger<PlayerService>> loggerMock;
    private PlayerService service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<R6trackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new R6trackerDbContext(options);
        loggerMock = new Mock<ILogger<PlayerService>>();
        service = new PlayerService(context, loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllPlayers()
    {
        context.R6Players.AddRange(
            new R6Player { Id = "1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" },
            new R6Player { Id = "2", PlayerName = "NovaSix", Platform = "PS5", Country = "US", UserId = "user1" }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllAsync_WithSearch_ReturnsFilteredPlayers()
    {
        context.R6Players.AddRange(
            new R6Player { Id = "1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" },
            new R6Player { Id = "2", PlayerName = "NovaSix", Platform = "PS5", Country = "US", UserId = "user1" }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync(search: "Shadow");

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().PlayerName, Is.EqualTo("ShadowR"));
    }

    [Test]
    public async Task GetAllAsync_WithPlatform_ReturnsFilteredPlayers()
    {
        context.R6Players.AddRange(
            new R6Player { Id = "1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" },
            new R6Player { Id = "2", PlayerName = "NovaSix", Platform = "PS5", Country = "US", UserId = "user1" }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync(platform: "PC");

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Platform, Is.EqualTo("PC"));
    }
    [Test]
    public async Task GetByIdAsync_WhenPlayerExists_ReturnsPlayer()
    {
        context.R6Players.Add(
            new R6Player { Id = "1", PlayerName = "ShadowR", Platform = "PC", Country = "BG", UserId = "user1" }
        );
        await context.SaveChangesAsync();

        var result = await service.GetByIdAsync("1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.PlayerName, Is.EqualTo("ShadowR"));
    }

    [Test]
    public void GetByIdAsync_WhenPlayerNotFound_ThrowsNotFoundException()
    {
        Assert.ThrowsAsync<R6tracker.Core.Exceptions.NotFoundException>(async () =>
            await service.GetByIdAsync("notexist"));
    }

    [Test]
    public async Task CreateAsync_ValidPlayer_AddsToDatabase()
    {
        var dto = new CreatePlayerDto
        {
            PlayerName = "TestPlayer",
            Platform = "PC",
            Level = 10,
            Country = "BG",
            MatchesPlayed = 0,
            Kills = 0,
            Deaths = 0
        };

        var result = await service.CreateAsync(dto, "user1");

        Assert.That(result.PlayerName, Is.EqualTo("TestPlayer"));
        Assert.That(context.R6Players.Count(), Is.EqualTo(1));
    }

}