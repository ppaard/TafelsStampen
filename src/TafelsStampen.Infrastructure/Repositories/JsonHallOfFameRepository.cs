namespace TafelsStampen.Infrastructure.Repositories;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;
using TafelsStampen.Infrastructure.JsonModels;

public class JsonHallOfFameRepository : JsonRepositoryBase<HallOfFameEntryJson>, IHallOfFameRepository
{
    public JsonHallOfFameRepository(FilePathProvider pathProvider)
        : base(pathProvider.HallOfFameFile) { }

    public async Task<IReadOnlyList<HallOfFameEntry>> GetByTableAsync(int tableNumber)
    {
        var all = await ReadAllAsync();
        return all.Where(e => e.TableNumber == tableNumber).Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<HallOfFameEntry>> GetAllAsync()
    {
        var all = await ReadAllAsync();
        return all.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(HallOfFameEntry entry)
    {
        var all = await ReadAllAsync();
        all.Add(MapToJson(entry));
        await WriteAllAsync(all);
    }

    private static HallOfFameEntry MapToDomain(HallOfFameEntryJson j) =>
        HallOfFameEntry.Reconstitute(j.Id, j.PlayerId, j.PlayerName, j.TableNumber, j.TotalTimeMs, j.ErrorCount, j.Date, Enum.Parse<GameMode>(j.Mode));

    private static HallOfFameEntryJson MapToJson(HallOfFameEntry e) => new()
    {
        Id = e.Id,
        PlayerId = e.PlayerId,
        PlayerName = e.PlayerName,
        TableNumber = e.TableNumber,
        TotalTimeMs = e.TotalTimeMs,
        ErrorCount = e.ErrorCount,
        Date = e.Date,
        Mode = e.Mode.ToString()
    };
}
