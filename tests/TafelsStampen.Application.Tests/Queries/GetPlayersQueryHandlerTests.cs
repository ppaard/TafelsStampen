namespace TafelsStampen.Application.Tests.Queries;
using Moq;
using Shouldly;
using TafelsStampen.Application.Queries.GetPlayers;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;

public class GetPlayersQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsAllPlayers()
    {
        var players = new List<Player> { Player.Create("Jan"), Player.Create("Kees") };
        var repo = new Mock<IPlayerRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

        var handler = new GetPlayersQueryHandler(repo.Object);
        var result = await handler.HandleAsync(new GetPlayersQuery());

        result.Count.ShouldBe(2);
        result[0].Name.ShouldBe("Jan");
        result[1].Name.ShouldBe("Kees");
    }
}
