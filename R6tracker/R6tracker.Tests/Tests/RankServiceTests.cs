using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using R6tracker.Core.Services;
using R6tracker.Infrastructure.Data;
using R6tracker.Infrastructure.Data.Models;

namespace R6tracker.Tests;

[TestFixture]
public class RankServiceTests
{
    private R6trackerDbContext context;
    private Mock<ILogger<RankService>> loggerMock;
    private RankService service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<R6trackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new R6trackerDbContext(options);
        loggerMock = new Mock<ILogger<RankService>>();
        service = new RankService(context, loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllRanks()
    {
        context.Ranks.AddRange(
            new Rank { Name = "Copper", Tier = 1 },
            new Rank { Name = "Bronze", Tier = 2 },
            new Rank { Name = "Silver", Tier = 3 }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public async Task GetAllAsync_ReturnsRanksOrderedByTier()
    {
        context.Ranks.AddRange(
            new Rank { Name = "Silver", Tier = 3 },
            new Rank { Name = "Copper", Tier = 1 },
            new Rank { Name = "Bronze", Tier = 2 }
        );
        await context.SaveChangesAsync();

        var result = await service.GetAllAsync();

        Assert.That(result.First().Name, Is.EqualTo("Copper"));
    }

    [Test]
    public async Task GetByIdAsync_WhenRankExists_ReturnsRank()
    {
        context.Ranks.Add(new Rank { Id = "rank-1", Name = "Gold", Tier = 4 });
        await context.SaveChangesAsync();

        var result = await service.GetByIdAsync("rank-1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Gold"));
    }

    [Test]
    public void GetByIdAsync_WhenRankNotFound_ThrowsNotFoundException()
    {
        Assert.ThrowsAsync<R6tracker.Core.Exceptions.NotFoundException>(async () =>
            await service.GetByIdAsync("notexist"));
    }
}