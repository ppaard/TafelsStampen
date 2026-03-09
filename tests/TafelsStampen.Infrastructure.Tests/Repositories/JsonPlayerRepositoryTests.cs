namespace TafelsStampen.Infrastructure.Tests.Repositories;
using Shouldly;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Infrastructure;
using TafelsStampen.Infrastructure.Repositories;

public class JsonPlayerRepositoryTests : IDisposable
{
    private readonly string _tempDir;
    private readonly JsonPlayerRepository _repo;

    public JsonPlayerRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _repo = new JsonPlayerRepository(new FilePathProvider(_tempDir));
    }

    [Fact]
    public async Task SaveAndGetById_ReturnsPlayer()
    {
        var player = Player.Create("Jan");
        await _repo.SaveAsync(player);
        var result = await _repo.GetByIdAsync(player.Id);
        result.ShouldNotBeNull();
        result!.Name.Value.ShouldBe("Jan");
    }

    [Fact]
    public async Task GetAll_ReturnsAllPlayers()
    {
        await _repo.SaveAsync(Player.Create("Jan"));
        await _repo.SaveAsync(Player.Create("Kees"));
        var all = await _repo.GetAllAsync();
        all.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsNull()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid());
        result.ShouldBeNull();
    }

    [Fact]
    public async Task Save_UpdatesExistingPlayer()
    {
        var player = Player.Create("Jan");
        await _repo.SaveAsync(player);
        await _repo.SaveAsync(player); // save again
        var all = await _repo.GetAllAsync();
        all.Count.ShouldBe(1); // should not duplicate
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
