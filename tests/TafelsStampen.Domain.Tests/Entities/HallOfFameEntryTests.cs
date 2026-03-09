namespace TafelsStampen.Domain.Tests.Entities;
using Shouldly;
using TafelsStampen.Domain.Entities;

public class HallOfFameEntryTests
{
    [Fact]
    public void Create_SetsAllPropertiesAndGeneratesId()
    {
        var playerId = Guid.NewGuid();
        var entry = new HallOfFameEntry(playerId, "Jan", 3, 45000, 2);

        entry.Id.ShouldNotBe(Guid.Empty);
        entry.PlayerId.ShouldBe(playerId);
        entry.PlayerName.ShouldBe("Jan");
        entry.TableNumber.ShouldBe(3);
        entry.TotalTimeMs.ShouldBe(45000);
        entry.ErrorCount.ShouldBe(2);
        entry.Date.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void Reconstitute_RestoresAllProperties()
    {
        var id = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var date = new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc);

        var entry = HallOfFameEntry.Reconstitute(id, playerId, "Kees", 5, 30000, 0, date);

        entry.Id.ShouldBe(id);
        entry.PlayerId.ShouldBe(playerId);
        entry.PlayerName.ShouldBe("Kees");
        entry.TableNumber.ShouldBe(5);
        entry.TotalTimeMs.ShouldBe(30000);
        entry.ErrorCount.ShouldBe(0);
        entry.Date.ShouldBe(date);
    }
}
