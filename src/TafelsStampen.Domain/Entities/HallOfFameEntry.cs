namespace TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.ValueObjects;

public class HallOfFameEntry
{
    public Guid Id { get; private set; }
    public Guid PlayerId { get; private set; }
    public string PlayerName { get; private set; }
    public int TableNumber { get; private set; }
    public long TotalTimeMs { get; private set; }
    public int ErrorCount { get; private set; }
    public DateTime Date { get; private set; }
    public GameMode Mode { get; private set; }

    private HallOfFameEntry() { }

    public HallOfFameEntry(Guid playerId, string playerName, int tableNumber, long totalTimeMs, int errorCount, GameMode mode = GameMode.Volgorde)
    {
        Id = Guid.NewGuid();
        PlayerId = playerId;
        PlayerName = playerName;
        TableNumber = tableNumber;
        TotalTimeMs = totalTimeMs;
        ErrorCount = errorCount;
        Mode = mode;
        Date = DateTime.UtcNow;
    }

    public static HallOfFameEntry Reconstitute(Guid id, Guid playerId, string playerName, int tableNumber, long totalTimeMs, int errorCount, DateTime date, GameMode mode = GameMode.Volgorde) =>
        new() { Id = id, PlayerId = playerId, PlayerName = playerName, TableNumber = tableNumber, TotalTimeMs = totalTimeMs, ErrorCount = errorCount, Date = date, Mode = mode };
}
