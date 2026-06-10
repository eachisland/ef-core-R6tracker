using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using R6tracker.Core.Services;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

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
}