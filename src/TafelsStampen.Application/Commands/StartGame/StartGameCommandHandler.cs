namespace TafelsStampen.Application.Commands.StartGame;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Repositories;
using TafelsStampen.Domain.ValueObjects;

public class StartGameCommandHandler : ICommandHandler<StartGameCommand, Guid>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly ILogger<StartGameCommandHandler> _logger;

    public StartGameCommandHandler(IGameSessionRepository sessionRepository, ILogger<StartGameCommandHandler> logger)
    {
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    public async Task<Guid> HandleAsync(StartGameCommand command)
    {
        _logger.LogDebug("Spelsessie starten voor speler {PlayerId}, tafel {TableNumber}, modus {Mode}",
            command.PlayerId, command.TableNumber, command.Mode);
        var session = new GameSession(command.PlayerId, new TableNumber(command.TableNumber), command.Mode);
        await _sessionRepository.SaveAsync(session);
        _logger.LogInformation("Spelsessie gestart: {SessionId}", session.Id);
        return session.Id;
    }
}
