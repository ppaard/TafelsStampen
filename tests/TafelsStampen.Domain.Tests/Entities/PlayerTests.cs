namespace TafelsStampen.Domain.Tests.Entities;
using Shouldly;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public class PlayerTests
{
    [Fact]
    public void Create_SetsNameAndGeneratesId()
    {
        var player = Player.Create("Jan");

        player.Id.ShouldNotBe(Guid.Empty);
        player.Name.Value.ShouldBe("Jan");
        player.CreatedAt.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void Create_TwoPlayers_HaveDifferentIds()
    {
        var p1 = Player.Create("Jan");
        var p2 = Player.Create("Kees");

        p1.Id.ShouldNotBe(p2.Id);
    }

    [Fact]
    public void Reconstitute_RestoresAllProperties()
    {
        var id = Guid.NewGuid();
        var name = new PlayerName("Lisa");
        var date = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc);

        var player = Player.Reconstitute(id, name, date);

        player.Id.ShouldBe(id);
        player.Name.Value.ShouldBe("Lisa");
        player.CreatedAt.ShouldBe(date);
    }
}
