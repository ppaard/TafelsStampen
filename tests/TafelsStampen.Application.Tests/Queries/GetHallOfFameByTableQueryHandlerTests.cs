namespace TafelsStampen.Application.Tests.Queries;
using Moq;
using Shouldly;
using TafelsStampen.Application.Queries.GetHallOfFameByTable;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;

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

        var handler = new GetHallOfFameByTableQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetHallOfFameByTableQuery(3));

        result.Count.ShouldBe(2);
        result[0].PlayerName.ShouldBe("Jan");
        result[0].Rank.ShouldBe(1);
        result[1].PlayerName.ShouldBe("Kees");
        result[1].Rank.ShouldBe(2);
    }
}
