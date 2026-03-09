namespace TafelsStampen.Application.DTOs;

public record HallOfFameEntryDto(
    int Rank,
    string PlayerName,
    int TableNumber,
    long TotalTimeMs,
    int ErrorCount,
    DateTime Date);
