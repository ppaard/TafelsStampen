namespace TafelsStampen.Application.DTOs;

public record GameResultDto(
    Guid SessionId,
    string PlayerName,
    int TableNumber,
    string Mode,
    long TotalTimeMs,
    int ErrorCount,
    IReadOnlyList<AnswerDto> Answers);
