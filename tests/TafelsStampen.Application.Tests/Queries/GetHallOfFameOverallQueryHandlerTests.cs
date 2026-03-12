namespace TafelsStampen.Application.Tests.Queries;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TafelsStampen.Application.Queries.GetHallOfFameOverall;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class GetHallOfFameOverallQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsSortedByTimeAcrossAllTables()
    {
        var entries = new List<HallOfFameEntry>
        {
            new(Guid.NewGuid(), "Kees", 5, 8000, 0),
            new(Guid.NewGuid(), "Jan",  3, 5000, 0),
            new(Guid.NewGuid(), "Lisa", 7, 6000, 1),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

        var handler = new GetHallOfFameOverallQueryHandler(repo.Object, NullLogger<GetHallOfFameOverallQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameOverallQuery());

        result.Count.ShouldBe(3);
        result[0].PlayerName.ShouldBe("Jan");
        result[0].Rank.ShouldBe(1);
        result[1].PlayerName.ShouldBe("Lisa");
        result[1].Rank.ShouldBe(2);
        result[2].PlayerName.ShouldBe("Kees");
        result[2].Rank.ShouldBe(3);
    }

    [Fact]
    public async Task HandleAsync_EmptyRepository_ReturnsEmptyList()
    {
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<HallOfFameEntry>());

        var handler = new GetHallOfFameOverallQueryHandler(repo.Object, NullLogger<GetHallOfFameOverallQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameOverallQuery());

        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_ModeFilter_ReturnsOnlyMatchingMode()
    {
        var entries = new List<HallOfFameEntry>
        {
            new(Guid.NewGuid(), "Jan",  3, 5000, 0, GameMode.Volgorde),
            new(Guid.NewGuid(), "Lisa", 7, 6000, 0, GameMode.Willekeurig),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

        var handler = new GetHallOfFameOverallQueryHandler(repo.Object, NullLogger<GetHallOfFameOverallQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameOverallQuery(GameMode.Willekeurig));

        result.Count.ShouldBe(1);
        result[0].PlayerName.ShouldBe("Lisa");
    }

    [Fact]
    public async Task HandleAsync_PlayerFilter_ReturnsOnlyMatchingPlayer()
    {
        var janId = Guid.NewGuid();
        var entries = new List<HallOfFameEntry>
        {
            new(janId,          "Jan",  3, 5000, 0),
            new(Guid.NewGuid(), "Lisa", 7, 6000, 0),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

        var handler = new GetHallOfFameOverallQueryHandler(repo.Object, NullLogger<GetHallOfFameOverallQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameOverallQuery(PlayerFilter: janId));

        result.Count.ShouldBe(1);
        result[0].PlayerName.ShouldBe("Jan");
    }
}
