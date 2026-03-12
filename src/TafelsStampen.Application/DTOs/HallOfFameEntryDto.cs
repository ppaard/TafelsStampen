namespace TafelsStampen.Application.DTOs;
using TafelsStampen.Domain.ValueObjects;

public record HallOfFameEntryDto(
    int Rank,
    string PlayerName,
    int TableNumber,
    long TotalTimeMs,
    int ErrorCount,
    DateTime Date,
    GameMode Mode);
