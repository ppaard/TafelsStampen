namespace TafelsStampen.Infrastructure.JsonModels;

public class HallOfFameEntryJson
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int TableNumber { get; set; }
    public long TotalTimeMs { get; set; }
    public int ErrorCount { get; set; }
    public DateTime Date { get; set; }
}
