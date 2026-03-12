namespace TafelsStampen.Application.Commands.FinishGame;
using Microsoft.Extensions.Logging;
using TafelsStampen.Application.Mediator;
using TafelsStampen.Domain.Entities;
using TafelsStampen.Domain.Exceptions;
using TafelsStampen.Domain.Repositories;

public class FinishGameCommandHandler : ICommandHandler<FinishGameCommand, Unit>
{
    private readonly IGameSessionRepository _sessionRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IHallOfFameRepository _hallOfFameRepository;
    private readonly ILogger<FinishGameCommandHandler> _logger;

    public FinishGameCommandHandler(
        IGameSessionRepository sessionRepository,
        IPlayerRepository playerRepository,
        IHallOfFameRepository hallOfFameRepository,
        ILogger<FinishGameCommandHandler> logger)
    {
        _sessionRepository = sessionRepository;
        _playerRepository = playerRepository;
        _hallOfFameRepository = hallOfFameRepository;
        _logger = logger;
    }

    public async Task<Unit> HandleAsync(FinishGameCommand command)
    {
        _logger.LogDebug("Spel afronden voor sessie {SessionId}", command.SessionId);

        var session = await _sessionRepository.GetByIdAsync(command.SessionId)
            ?? throw new DomainException($"Sessie {command.SessionId} niet gevonden.");

        var player = await _playerRepository.GetByIdAsync(session.PlayerId)
            ?? throw new DomainException($"Speler niet gevonden.");

        session.Finish();
        await _sessionRepository.SaveAsync(session);

        var entry = new HallOfFameEntry(
            session.PlayerId,
            player.Name.Value,
            session.TableNumber.Value,
            session.TotalTimeMs,
            session.ErrorCount,
            session.Mode);
        await _hallOfFameRepository.SaveAsync(entry);

        _logger.LogInformation(
            "Spel afgerond: sessie {SessionId}, speler {PlayerName}, tafel {TableNumber}, tijd {TotalTimeMs}ms, fouten {ErrorCount}",
            session.Id, player.Name.Value, session.TableNumber.Value, session.TotalTimeMs, session.ErrorCount);

        return Unit.Value;
    }
}
