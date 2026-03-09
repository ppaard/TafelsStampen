namespace TafelsStampen.Infrastructure.Tests.Repositories;
using Shouldly;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Infrastructure;
using TafelsStampen.Infrastructure.Repositories;

public class JsonHallOfFameRepositoryTests : IDisposable
{
    private readonly string _tempDir;
    private readonly JsonHallOfFameRepository _repo;

    public JsonHallOfFameRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _repo = new JsonHallOfFameRepository(new FilePathProvider(_tempDir));
    }

    [Fact]
    public async Task Save_AndGetByTable_ReturnsCorrectEntries()
    {
        await _repo.SaveAsync(new HallOfFameEntry(Guid.NewGuid(), "Jan", 3, 5000, 1));
        await _repo.SaveAsync(new HallOfFameEntry(Guid.NewGuid(), "Kees", 5, 3000, 0));

        var table3 = await _repo.GetByTableAsync(3);
        table3.Count.ShouldBe(1);
        table3[0].PlayerName.ShouldBe("Jan");
    }

    [Fact]
    public async Task GetAll_ReturnsAllEntries()
    {
        await _repo.SaveAsync(new HallOfFameEntry(Guid.NewGuid(), "Jan", 3, 5000, 1));
        await _repo.SaveAsync(new HallOfFameEntry(Guid.NewGuid(), "Kees", 5, 3000, 0));

        var all = await _repo.GetAllAsync();
        all.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetByTable_NoMatches_ReturnsEmpty()
    {
        await _repo.SaveAsync(new HallOfFameEntry(Guid.NewGuid(), "Jan", 3, 5000, 1));
        var result = await _repo.GetByTableAsync(7);
        result.Count.ShouldBe(0);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
