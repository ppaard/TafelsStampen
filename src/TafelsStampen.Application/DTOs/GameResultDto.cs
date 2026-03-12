namespace TafelsStampen.Application.DTOs;

public record GameResultDto(
    Guid SessionId,
    Guid PlayerId,
    string PlayerName,
    int TableNumber,
    string Mode,
    long TotalTimeMs,
    int ErrorCount,
    IReadOnlyList<AnswerDto> Answers);
