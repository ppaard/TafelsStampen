namespace TafelsStampen.Application.DTOs;

public record AnswerDto(int Multiplicand, int Multiplier, int GivenAnswer, int CorrectAnswer, bool IsCorrect, long ReactionTimeMs);
