namespace TafelsStampen.Application.Commands.StartGame;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class StartGameCommandHandler : ICommandHandler<StartGameCommand, Guid>
{
    private readonly IGameSessionRepository _sessionRepository;

    public StartGameCommandHandler(IGameSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Guid> HandleAsync(StartGameCommand command)
    {
        var session = new GameSession(command.PlayerId, new TableNumber(command.TableNumber), command.Mode);
        await _sessionRepository.SaveAsync(session);
        return session.Id;
    }
}
