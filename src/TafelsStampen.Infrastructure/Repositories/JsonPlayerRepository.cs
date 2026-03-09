namespace TafelsStampen.Infrastructure.Repositories;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Infrastructure.JsonModels;

public class JsonPlayerRepository : JsonRepositoryBase<PlayerJson>, IPlayerRepository
{
    public JsonPlayerRepository(FilePathProvider pathProvider)
        : base(pathProvider.PlayersFile) { }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        var all = await ReadAllAsync();
        var match = all.FirstOrDefault(p => p.Id == id);
        return match is null ? null : MapToDomain(match);
    }

    public async Task<IReadOnlyList<Player>> GetAllAsync()
    {
        var all = await ReadAllAsync();
        return all.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(Player player)
    {
        var all = await ReadAllAsync();
        var idx = all.FindIndex(p => p.Id == player.Id);
        var json = MapToJson(player);
        if (idx >= 0) all[idx] = json;
        else all.Add(json);
        await WriteAllAsync(all);
    }

    private static Player MapToDomain(PlayerJson j) =>
        Player.Reconstitute(j.Id, new PlayerName(j.Name), j.CreatedAt);

    private static PlayerJson MapToJson(Player p) => new()
    {
        Id = p.Id,
        Name = p.Name.Value,
        CreatedAt = p.CreatedAt
    };
}
