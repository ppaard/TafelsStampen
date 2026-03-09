namespace TafelsStampen.Application.Commands.SubmitAnswer;
using TafelsStampen.Application.Mediator;

public record SubmitAnswerCommand(Guid SessionId, int Multiplicand, int Multiplier, int GivenAnswer, long ReactionTimeMs) : ICommand<bool>;
