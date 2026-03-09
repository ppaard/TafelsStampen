namespace TafelsStampen.Application.Commands.RegisterPlayer;
using TafelsStampen.Application.Mediator;

public record RegisterPlayerCommand(string Name) : ICommand<Guid>;
