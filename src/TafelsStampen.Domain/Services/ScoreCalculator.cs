namespace TafelsStampen.Domain.Services;
using TafelsStampen.Domain.Entities;

public static class ScoreCalculator
{
    public static long CalculateTotalTime(GameSession session) => session.TotalTimeMs;
    public static int CalculateErrors(GameSession session) => session.ErrorCount;

    // Ranking: lower total time = better rank, ties broken by fewer errors
    public static int CalculateRank(HallOfFameEntry entry, IEnumerable<HallOfFameEntry> allEntries)
    {
        var sorted = allEntries
            .OrderBy(e => e.TotalTimeMs)
            .ThenBy(e => e.ErrorCount)
            .ToList();
        return sorted.FindIndex(e => e.Id == entry.Id) + 1;
    }
}
