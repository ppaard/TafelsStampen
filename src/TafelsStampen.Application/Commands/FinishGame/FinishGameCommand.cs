namespace TafelsStampen.Application.Commands.FinishGame;
using TafelsStampen.Application.Mediator;

public record FinishGameCommand(Guid SessionId) : ICommand<Unit>;
