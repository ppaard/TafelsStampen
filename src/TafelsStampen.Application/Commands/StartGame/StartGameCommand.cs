namespace TafelsStampen.Application.Commands.StartGame;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.ValueObjects;

public record StartGameCommand(Guid PlayerId, int TableNumber, GameMode Mode) : ICommand<Guid>;
