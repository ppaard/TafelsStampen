namespace TafelsStampen.Application.Tests.Queries;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class GetHallOfFameByTableQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsSortedByTime()
    {
        var entries = new List<HallOfFameEntry>
        {
            new(Guid.NewGuid(), "Kees", 3, 5000, 1),
            new(Guid.NewGuid(), "Jan", 3, 3000, 0),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetByTableAsync(3)).ReturnsAsync(entries);

        var handler = new GetHallOfFameByTableQueryHandler(repo.Object, NullLogger<GetHallOfFameByTableQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameByTableQuery(3));

        result.Count.ShouldBe(2);
        result[0].PlayerName.ShouldBe("Jan");
        result[0].Rank.ShouldBe(1);
        result[1].PlayerName.ShouldBe("Kees");
        result[1].Rank.ShouldBe(2);
    }

    [Fact]
    public async Task HandleAsync_ModeFilter_ReturnsOnlyMatchingMode()
    {
        var entries = new List<HallOfFameEntry>
        {
            new(Guid.NewGuid(), "Jan",  3, 5000, 0, GameMode.Volgorde),
            new(Guid.NewGuid(), "Lisa", 3, 6000, 0, GameMode.Willekeurig),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetByTableAsync(3)).ReturnsAsync(entries);

        var handler = new GetHallOfFameByTableQueryHandler(repo.Object, NullLogger<GetHallOfFameByTableQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameByTableQuery(3, GameMode.Volgorde));

        result.Count.ShouldBe(1);
        result[0].PlayerName.ShouldBe("Jan");
    }

    [Fact]
    public async Task HandleAsync_PlayerFilter_ReturnsOnlyMatchingPlayer()
    {
        var janId = Guid.NewGuid();
        var entries = new List<HallOfFameEntry>
        {
            new(janId,          "Jan",  3, 3000, 0),
            new(Guid.NewGuid(), "Lisa", 3, 4000, 0),
        };
        var repo = new Mock<IHallOfFameRepository>();
        repo.Setup(r => r.GetByTableAsync(3)).ReturnsAsync(entries);

        var handler = new GetHallOfFameByTableQueryHandler(repo.Object, NullLogger<GetHallOfFameByTableQueryHandler>.Instance);
        var result = await handler.HandleAsync(new GetHallOfFameByTableQuery(3, PlayerFilter: janId));

        result.Count.ShouldBe(1);
        result[0].PlayerName.ShouldBe("Jan");
    }
}
